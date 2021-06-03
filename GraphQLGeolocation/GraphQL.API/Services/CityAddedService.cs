using System;
using System.Reactive.Subjects;
using System.Linq;
using System.Reactive.Linq;
using GraphQL.API.Messages;

namespace GraphQL.API.Services
{
    public class CityAddedService
    {
        private readonly ISubject<CityAddedMessage> _messageStream = new ReplaySubject<CityAddedMessage>(1);
        
        public CityAddedMessage AddCityAddedMessage(CityAddedMessage message)
        {
            _messageStream.OnNext(message);
            return message;
        }

        public IObservable<CityAddedMessage> GetMessages(string countryName)
        {
            var mess = _messageStream
                .Where(message =>
                    message.CountryName == countryName
                ).Select(s => s)
                .AsObservable();

            return mess;
        }
    }
}