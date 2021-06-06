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

Kratak pregled tipa entiteta koji opisuju gradove. Asinhrona polja se pribavljaju pozivom konkretnih repozitorijuma.

```csharp
    public class CityType : ObjectGraphType<City>
    {
        public IServiceProvider Provider { get; set; }
        public CityType(IServiceProvider provider)
        {
            Field(x => x.Id, type: typeof(IntGraphType));
            Field(x => x.Name, type: typeof(StringGraphType));
            Field(x => x.Population, type: typeof(IntGraphType));
            Field<CountryType>("country", resolve: context => {
                IGenericRepository<Country> countryRepository = (IGenericRepository<Country>)provider.GetService(typeof(IGenericRepository<Country>));
                return countryRepository.GetById(context.Source.CountryId);
            });
        }
    }
```

#### Primer mutacije

Mutacije se zadaju kao akcije ulaznih argumenata upita, može se videti ulaz mutacije za dodavanje gradova `addCity`. Po sličnoj analogiji se zadaju i upiti za čitanje, kakav je na primer `cities`.

```csharp
public class AddCityMutation : IFieldMutationServiceItem
{
    public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
    {
        objectGraph.Field<CityType>("addCity",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "countryId"},
                new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "cityName"},
                new QueryArgument<IntGraphType> {Name = "population"}
            ),
            resolve: context =>
            ...
```

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
