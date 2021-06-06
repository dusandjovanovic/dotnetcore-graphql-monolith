# GraphQL Monolith

### Primeri `query-a`

#### Lista država

```
query countries{
  countries{
    id
    name
  }
}
```

#### Lista gradova

```
query cities{
  cities{
    id
    name
    population
    country{
      id
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
    id
    name
    email
    dateOfBirth
  }
}
```

#### Dodavanje država

```
mutation addCountry{
  addCountry(countryName:"Serbia"){
    id
    name
  }
}
```

#### Dodavanje gradova

```
mutation addCityToSerbia{
  addCity(countryId:1,cityName:"Belgrade",population:22200000){
    id
    name
    population
    country{
      id
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
    name,
  }
}
```

#### Dodavanje recenzija

```
mutation addReview{
  addReview(description: "Something you have to say about it", placeId: 1, accountId: 1){
    id
    name
  }
}
```

#### Dodavanje naloga

```
mutation addAccount{
  addAccount(name: "Name LastName", email: "myemail@mail.com", dateOfBirth: "30/12/2021 05:50"){
    id
    name
  }
}
```

#### Subscription na `cityAdded`

Otvoriti subscription u drugom pretraživaču i pratiti promene. Prilikom izdavanja novih `query-a` za dodavanje gradova, promene će biti praćene.

```
subscription cityAddedToGermany{
  cityAdded(countryName:"Germany"){
    id
    cityName
    countryName
    message
  }
}
```
