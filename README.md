# GraphQL Monolith

GraphQL API koji se koristi za deljenje *location-based* sadržaja izmedju korisnika. APi podržava dodavanje zemalja, gradova, mesta, korisnika i recenzija koje se vezuju za korisnike/mesta uz tačne koordinate.

## Pregled osnovne arhitekture sistema

```
/
  Solution/
    GraphQL.API/
    GraphQL.Core/
    GraphQL.Data/
```

`GraphQL.Core` projekat definiše osnovne modele.

`GraphQL.Data` projekat definiše sloj perzistencije. 
Za perzistenciju se koristi `mssql_server` baza podataka, dok je mapiranje ostvareno korišćenjem rešenja `EntityFrameworkCore`. Mapiranje se ostvaruje kroz jedan kontekst. Pristup entitetima konteksta različitih tipova moguć je samo kroz repozitorijume. Postoji osnovni generički repozitorijum `GenericRepository<T>` i po potrebi se proširuje dodatnim metodama.

Za komunikaciju sa izvorom podataka neophodno je navesti parametre u stringu koji opisuje konekciju koji će ostvariti vezu sa *driver-om*. Ukoliko se koristi `docker` kontejner `mcr.microsoft.com/mssql/server:2019-latest` sa podrazumevanim podešavanjima parametar konekcije je `Server=localhost,1433;Database=medical;MultipleActiveResultSets=true;User=sa;Password=yourStrong(!)Password`.

```csharp
public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration) =>
    services
        .AddDbContext<ApplicationContext>(options => options.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("GraphQL.API")))
        .AddDatabaseDeveloperPageExceptionFilter();
```

`GraphQL.API` projekat definiše API baziran na GraphQL schemi. Svi tipovi, mutacije, subscriptioni, upiti i sama schema se nalaze u ovom projektu. Kontroler poseduje samo jednu tačku koja parsuje upite i prosledjuje upravljanje *executoru*.

#### Primer tipa

Kratak pregled tipa entiteta koji opisuju mesta. Asinhrona polja (poput `location` i `city`) se pribavljaju pozivom konkretnih repozitorijuma.

```csharp
public class PlaceType : ObjectGraphType<Place>
  {
      public IServiceProvider Provider { get; set; }

      public PlaceType(IServiceProvider provider)
      {
          Field(x => x.Id, type: typeof(IntGraphType));
          Field(x => x.Name, type: typeof(StringGraphType));
          Field<LocationType>("location", resolve: context => {
              IGenericRepository<Location> locationRepository = (IGenericRepository<Location>)provider.GetService(typeof(IGenericRepository<Location>));
              return locationRepository.GetById(context.Source.LocationId);
          });
          Field<CityType>("city", resolve: context => {
              IGenericRepository<City> cityRepository = (IGenericRepository<City>)provider.GetService(typeof(IGenericRepository<City>));
              return cityRepository.GetById(context.Source.CityId);
          });
      }
  }
```

#### Primer upita

Upiti se koriste za pribavljanje entiteta odredjenog tipa. Tako, na primer, `place` upit može da se koristi za pribavljanje mesta gde se preko repozitorjiuma pristupa kontekstu. Pribavljena mesta se mogu dodatno filtrirati `name` argumentom za koji se očekuje poklapanje.

```csharp
public class PlaceQuery : IFieldQueryServiceItem
{
    public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
    {
        objectGraph.Field<ListGraphType<PlaceType>>("places",
            arguments: new QueryArguments(
                new QueryArgument<StringGraphType> {Name = "name"}
            ),
            resolve: context =>
            {
                var placeRepository = (IGenericRepository<Place>) sp.GetService(typeof(IGenericRepository<Place>));
                var baseQuery = placeRepository.GetAll();
                var name = context.GetArgument<string>("name");
                return name != default(string) ? baseQuery.Where(w => w.Name.Contains(name)) : baseQuery.ToList();
            });
    }
}
```

#### Primer mutacije

Mutacije se zadaju kao akcije ulaznih argumenata upita, može se videti ulaz mutacije za dodavanje mesta `addPlace`. Repozitorijumi se kao i u prethodnom slučaju probavljaju od Dependency Injection kontejnera i koriste za promenu konteksta.

```csharp
public class AddPlaceMutation : IFieldMutationServiceItem
{
    public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
    {
        objectGraph.Field<PlaceType>("addPlace",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "cityId"},
                new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "placeName"},
                new QueryArgument<FloatGraphType> {Name = "latitude"},
                new QueryArgument<FloatGraphType> {Name = "longitude"}
            ),
            resolve: context =>
            {
                var placeName = context.GetArgument<string>("placeName");
                var cityId = context.GetArgument<int>("cityId");
                var latitude = context.GetArgument<double>("latitude");
                var longitude = context.GetArgument<double>("longitude");

                var placeRepository = (IGenericRepository<Place>) sp.GetService(typeof(IGenericRepository<Place>));
                var locationRepository = (IGenericRepository<Location>) sp.GetService(typeof(IGenericRepository<Location>));
                ...
```

Dodatno, postoji i jedan subscription koji se koristi prilikom dodavanja gradova. Slanjem poruka preko magistrale se obaveštavaju ostali korisnici API-a da je došlo do promene - u slučaju mutacije `addCity` se tako šalje poruka `CityAddedMessage`.

`FieldService` servis vodi računa o registrovanju svih GraphQL tipova, mutacija i upita. Nije neophodno ručno registrovati nove upite, već se iz sadržaja assembly-a svi registruju, sudeći po njihovim osnovnim tipovima.

### Autentikacija i autorizacija

Za autentikaciju se koristi SSO provajdera `Okta` i neophodno je konfigurisati domen u podešavanjima. Pritom, validacija autentikacije se oslanja na *Bearer* JSON web-tokene koji će biti neophodni u zaglavljima.

```csharp
public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration) =>
    services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
            options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
            options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
        })
        .AddOktaWebApi(new OktaWebApiOptions()
        {
            OktaDomain = configuration
                .GetSection(nameof(ApplicationOptions.Authentication))
                .Get<AuthenticationOptions>().Domain
        }).Services;
```

Neophodno je pribaviti token da bi se pristupilo APi-u. `https://{okta-url}/oauth2/default/v1/token` je primer endpointa za dobijanje tokena. Očekuje se da se domen `Okta` provajdera slaže sa domenom navedenim u `Configuration["Authentication"]`.

### Dependency Injection kontejner

Direktorijum `/Extensions` sadrži extension metode za registraciju servisa u Dependency injection kontejner.

* `AddProjectServices` za registrovanje potrebnih servisa
* `AddProjectRepositories` za registrovanje repozitorijuma koji se koriste za pristup entitetima konteksta
* `AddProjectSchema` za dodavanje svih potrebnih GraphQL tipova
* `AddDbContext` za povezivanje sa `mssql_server` bazom podataka
* `AddCustomCaching` za dodavanje podrške keširanja
* `AddCustomCors` za konfigurisanje CORS pravila
* `AddCustomOptions` za registrovanje podešavanja iz `appsettings.json` datoteka
* `AddCustomResponseCompression` za dodavanje servisa za kompresiju poput `gzip` podrške
* `AddCustomRouting` za podešavanje rutiranja (lowercase rute)
* `AddCustomHealthChecks` za dodavanje healthcheck servisa u kontejner
* `AddCustomGraphQL` za registrovanje svih potrebnih servisa GrphQL-a poput dodavanja tipova, omogućavanja socketa
* `AddAuthorization` za dodavanje autorizacije `Okta` Single-Sign-On provajderom
* `AddAuthorizationValidation` za validaciju autorizaije

### Middleware

Pipeline za obradu zahteva je proširen middleware delegatima.

* `UseServerTiming` za dodavanje zaglavlja o potrebnom vremenu obrade zahteva (samo u developmentu)
* `UseDeveloperExceptionPage` za generisanje html stranica u slučaju grešaka (samo u developmentu)
* `UseGraphQLPlayground`za development i testiranje APi-a, `UseGraphQLVoyager` za produkciju
* `UseEndpoints` za dodavanje `/status` endpointa u vidu healthchecka
* `UseAuthentication` i `UseAuthorization` za autentikaciju
* `UseResponseCompression` za kompresiju (primetno kod `json` datoteka npr)
* `UseStaticFilesWithCacheControl` za serviranje statičkih datoteka sa podešavanjem iz `Configuration["CacheProfiles"]`

## Pokretanje sistema

Sistem se pokreće nakon "izgradnje" pod-projekta `GraphQL.API`.

Neophodno je pre svega izvršiti migracije nad bazom podataka:

`$ dotnet ef migrations add InitialCreate`

`$ dotnet ef database update`

Migracije se vrše iz pomenutog API pod-projekta jer je označen kao `MigrationsAssembly`, iako su konteksti napisani u domenskom pod-projektu.

Na kraju, aplikativni sloj sadrži i `graphql-playground` interfejs preko koga se mogu slati upiti.

![alt text][user_interface]

[user_interface]: Docs/screenshot-playground.png

### Primeri `query-a`

#### Lista država

```
query countries{
  countries{
    id,
    name
  }
}
```

#### Lista gradova

```
query cities{
  cities{
    id,
    name,
    population,
    country{
      id,
      name
    }
  }
}
```

#### Lista označenih mesta

```
query places{
  places{
    id,
    name,
    location {
      latitude,
      longitude
    },
    city {
      id,
      name,
      population
    }
  }
}
```

#### Lista recenzija

```
query reviews{
  reviews{
    id,
    description,
    account {
      id,
      name,
      email
    },
    place {
      id,
      name
    }
  }
}
```

#### Lista korisnika

```
query accounts{
  accounts{
    id,
    name,
    email,
    dateOfBirth
  }
}
```

#### Dodavanje država

```
mutation addCountry{
  addCountry(countryName:"Serbia"){
    id,
    name
  }
}
```

#### Dodavanje gradova

```
mutation addCityToSerbia{
  addCity(countryId:1,cityName:"Belgrade",population:22200000){
    id,
    name,
    population,
    country{
      id,
      name
    }
  }
}
```

#### Dodavanje "označenih" mesta
```
mutation addPlace{
  addPlace(cityId: 1, placeName: "Some Tower in the center", latitude: 40.1231231234, longitude: 42.1231231233){
    id,
    name
  }
}
```

#### Dodavanje recenzija

```
mutation addReview{
  addReview(description: "Something you have to say about it", placeId: 1, accountId: 1){
    id,
    description
  }
}
```

#### Dodavanje naloga

```
mutation addAccount{
  addAccount(name: "Name LastName", email: "myemail@mail.com", dateOfBirth: "30/12/2021 05:50"){
    id,
    name
  }
}
```

#### Dodavanje prijatelja

```
mutation addFriend{
  addFriend(sourceId:1, destinationId: 2){
    id,
    name
  }
}
```

#### Brisanje prijatelja

```
mutation removeFriend{
  removeFriend(sourceId:1, destinationId: 2){
    id,
    name
  }
}
```

#### Subscription na `cityAdded`

Otvoriti subscription u drugom pretraživaču i pratiti promene. Prilikom izdavanja novih `query-a` za dodavanje gradova, promene će biti praćene.

```
subscription cityAddedToGermany{
  cityAdded(countryName:"Germany"){
    id,
    cityName,
    countryName,
    message
  }
}
```
