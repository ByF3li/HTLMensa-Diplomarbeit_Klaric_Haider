﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MensaAppKlassenBibliothek
{
    public class Person
    {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }

        public List<MenuPerson> MenuPersons { get; set; } = new List<MenuPerson>() { };


        public async void SaveObject()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            // Serialize the object to a string
            string serializedObject = JsonSerializer.Serialize(this, options);

            // Store the serialized object in Preferences
            await SecureStorage.SetAsync("userEmail", serializedObject);
        }

        public async static Task<Person> LoadObject()
        {
            // Retrieve the serialized object from Preferences
            
                // Deserialize the string back to the object
            string serializedObject = await SecureStorage.GetAsync("userEmail");
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            return JsonSerializer.Deserialize<Person>(serializedObject, options);
            

        }
    }
}