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
