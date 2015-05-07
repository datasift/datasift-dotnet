﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataSift;
using Newtonsoft.Json;
using DataSift.Rest;

namespace QuickStartPylon
{
    class Program
    {
        private static DataSiftClient _client;

        static void Main(string[] args)
        {
            // Create a new DataSift client
            _client = new DataSiftClient("DATASIFT_USERNAME", "DATASIFT_APIKEY");

            var hash = StartRecording();
            Analyze(hash);

            // Wait for key press before ending example
            Console.WriteLine("-- Press any key to exit --");
            Console.ReadKey(true);
        }

        static string StartRecording()
        {
            // Compile a filter to receive a hash
            var csdl = @"( fb.content contains_any ""wedding,engaged,engagement,marriage"" 
		                or fb.topics.name in ""Wedding,Marriage"" ) 
		                OR ( fb.parent.content contains_any ""wedding,engaged,engagement,marriage"" 
		                or fb.parent.topics.name in ""Wedding,Marriage"" )";

            var compiled = _client.Pylon.Compile(csdl);
            var hash = compiled.Data.hash;

            Console.WriteLine("Hash: " + hash);

            // Start the recording, 
            try
            {
                _client.Pylon.Start(hash, "Pylon Test Filter");
            }
            catch(RestAPIException ex)
            {
                // If the recording is already running the API will return a 409 status, we can ignore this in the example
                if (ex.Response.StatusCode == System.Net.HttpStatusCode.Conflict) { }
                else throw;
            }
            
            Console.WriteLine("Recording running!");

            // Return hash for analysis later
            return hash;
        }

        static void Analyze(string hash)
        {
            var analysisParams = new
            {
                analysis_type = "freqDist",
                parameters = new
                {
                    threshold = 5,
                    target = "fb.author.age"
                }
            };

            var analysis = _client.Pylon.Analyze(hash, analysisParams);
            Console.WriteLine("\nAnalysis result: " + JsonConvert.SerializeObject(analysis.Data));

            var analysisWithFilter = _client.Pylon.Analyze(hash, analysisParams, filter: "fb.author.gender == \"female\" OR fb.parent.author.gender == \"female\"");
            Console.WriteLine("\nAnalysis (with filter) result: " + JsonConvert.SerializeObject(analysisWithFilter.Data));
        }

    }
}