﻿using Newtonsoft.Json;
using PeopleViewer.Common;
using System.Collections.Generic;
using System.Net;

namespace PersonDataReader.Service
{
    public class ServiceReader: IPersonReader
    {
        private readonly WebClient client = new WebClient();
        private const string baseUri = "http://localhost:9874/api/people";

        public IEnumerable<Person> GetPeople()
        {
            var result = client.DownloadString(baseUri);
            var people = JsonConvert.DeserializeObject<IEnumerable<Person>>(result);
            return people;
        }

        public Person GetPerson(int id)
        {
            var result = client.DownloadString($"{baseUri}/{id}");
            var person = JsonConvert.DeserializeObject<Person>(result);
            return person;
        }
    }
}
