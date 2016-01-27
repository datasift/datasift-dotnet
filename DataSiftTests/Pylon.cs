﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace DataSiftTests
{
    [TestClass]
    public class Pylon : TestBase
    {

        private const string VALID_CSDL = "fb.content contains_any \"BMW, Mercedes, Cadillac\"";
        private const string VALID_ID = "77eb8c4b74257406547ab1ed3be346b6";
        private const string VALID_HASH = "58eb8c4b74257406547ab1ed3be346a8";
        private const string VALID_NAME = "Example recording";
        private DateTimeOffset VALID_START = DateTimeOffset.Now.AddDays(-30);
        private DateTimeOffset VALID_END = DateTimeOffset.Now;

        public dynamic DummyParameters
        {
            get
            {
                return new
                {
                    analysis_type = "freqDist",
                    parameters = new
                    {
                        threshold = 5,
                        target = "fb.author.age"
                    }
                };
            }
        }

        #region Get

        [TestMethod]
        public void Get_Succeeds()
        {
            var response = Client.Pylon.Get();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_ID_Empty_Fails()
        {
            Client.Pylon.Get(id: "");
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_ID_Bad_Format_Fails()
        {
            Client.Pylon.Get(id: "invalid");
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Get_By_ID_Complete_Succeeds()
        {
            var response = Client.Pylon.Get(id: VALID_ID);
            Assert.AreEqual(VALID_ID, response.Data.hash);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_Page_Is_Less_Than_One_Fails()
        {
            Client.Pylon.Get(page: 0);
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Get_Page_Succeeds()
        {
            var response = Client.Pylon.Get(page: 1);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_Per_Page_Is_Less_Than_One_Fails()
        {
            Client.Pylon.Get(perPage: 0);
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Get_PerPage_Succeeds()
        {
            var response = Client.Pylon.Get(page: 1, perPage: 1);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        #endregion

        #region Validate

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Validate_Null_CSDL_Fails()
        {
            Client.Pylon.Validate(null);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_Empty_CSDL_Fails()
        {
            Client.Pylon.Validate("");
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Validate_Complete_CSDL_Succeeds()
        {
            var response = Client.Pylon.Validate(VALID_CSDL);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        #endregion

        #region Compile

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Compile_Null_CSDL_Fails()
        {
            Client.Pylon.Compile(null);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Compile_Empty_CSDL_Fails()
        {
            Client.Pylon.Compile("");
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Compile_Complete_CSDL_Succeeds()
        {
            var response = Client.Pylon.Compile(VALID_CSDL);
            Assert.AreEqual("58eb8c4b74257406547ab1ed3be346a8", response.Data.hash);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        #endregion

        #region Start

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Start_Null_Hash_Fails()
        {
            Client.Pylon.Start(null, VALID_NAME);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Start_Empty_Hash_Fails()
        {
            Client.Pylon.Start("", VALID_NAME);
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Start_Null_Name_Succeeds()
        {
            var response = Client.Pylon.Start(VALID_HASH, null);
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Start_Empty_Name_Fails()
        {
            var response = Client.Pylon.Start(VALID_HASH, "");
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Start_Succeeds()
        {
            var response = Client.Pylon.Start(VALID_HASH, VALID_NAME);
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            throw new NotImplementedException();
        }

        #endregion

        #region Stop

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Stop_Null_ID_Fails()
        {
            Client.Pylon.Stop(null);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Stop_Empty_ID_Fails()
        {
            Client.Pylon.Stop("");
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Stop_Succeeds()
        {
            var response = Client.Pylon.Stop(VALID_ID);
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            throw new NotImplementedException();
        }

        #endregion

        #region Analyze

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyze_Null_ID_Fails()
        {
            Client.Pylon.Analyze(null, DummyParameters);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Analyze_Empty_ID_Fails()
        {
            Client.Pylon.Analyze("", DummyParameters);
            throw new NotImplementedException();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Analyze_Empty_Filter_Fails()
        {
            Client.Pylon.Analyze(VALID_ID, DummyParameters, filter: "");
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Analyze_With_Null_Filter_Succeeds()
        {
            var response = Client.Pylon.Analyze(VALID_ID, DummyParameters, filter: null);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Analyze_With_Filter_Succeeds()
        {
            var response = Client.Pylon.Analyze(VALID_ID, DummyParameters, filter: "interaction.content contains 'apple'");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Analyze_Too_Late_Start_Fails()
        {
            Client.Pylon.Analyze(VALID_ID, DummyParameters, start: DateTimeOffset.Now.AddDays(1), end: DateTimeOffset.Now.AddDays(3));
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Analyze_Too_Late_End_Fails()
        {
            Client.Pylon.Analyze(VALID_ID, DummyParameters, start: VALID_START, end: DateTimeOffset.Now.AddDays(1));
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Analyze_End_Before_Start_Fails()
        {
            Client.Pylon.Analyze(VALID_ID, DummyParameters, start: VALID_START, end: DateTimeOffset.Now.AddDays(-31));
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Analyze_With_Null_Start_Succeeds()
        {
            var response = Client.Pylon.Analyze(VALID_ID, DummyParameters, start: null, end: DateTimeOffset.Now);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Analyze_With_Null_End_Succeeds()
        {
            var response = Client.Pylon.Analyze(VALID_ID, DummyParameters, start: DateTimeOffset.Now.AddDays(-1), end: null);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Analyze_With_Start_And_End_Succeeds()
        {
            var response = Client.Pylon.Analyze(VALID_ID, DummyParameters, start: DateTimeOffset.Now.AddDays(-1), end: DateTimeOffset.Now);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Analyze_With_Null_Parameters_Fails()
        {
            Client.Pylon.Analyze(VALID_ID, parameters: null, start: DateTimeOffset.Now.AddDays(-1), end: DateTimeOffset.Now);
            throw new NotImplementedException();
        }


        [TestMethod]
        public void Analyze_Succeeds()
        {
            var response = Client.Pylon.Analyze(VALID_ID, DummyParameters);
            Assert.AreEqual(false, response.Data.analysis.redacted);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Analyze_Nested()
        {
            dynamic nested = new {
                    analysis_type = "freqDist",
                    parameters = new
                    {
                        threshold = 3,
                        target = "fb.author.gender"
                    },
                    child = new {
                        parameters = new
                        {
                            threshold = 3,
                            target = "fb.author.age"
                        }
                    }
                };

            var response = Client.Pylon.Analyze("58eb8c4b74257406547ab1ed3bnested", nested);
            Assert.AreEqual(false, response.Data.analysis.redacted);
            Assert.AreEqual("freqDist", response.Data.analysis.results[0].child.analysis_type);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        #endregion

        #region Tags

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Tags_Null_ID_Fails()
        {
            Client.Pylon.Tags(null);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tags_Empty_ID_Fails()
        {
            Client.Pylon.Tags("");
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tags_Invalid_ID_Fails()
        {
            Client.Pylon.Tags("invalid");
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Tags_Succeeds()
        {
            var response = Client.Pylon.Tags(VALID_ID);
            Assert.AreEqual("tag.one", response.Data[0]);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        #endregion

        #region Sample

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Sample_Null_ID_Fails()
        {
            Client.Pylon.Sample(null);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Sample_Empty_ID_Fails()
        {
            Client.Pylon.Sample("");
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Sample_ID_Bad_Format_Fails()
        {
            Client.Pylon.Sample("invalid");
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Sample_Too_Low_Count_Fails()
        {
            Client.Pylon.Sample(VALID_ID, 9);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Sample_Too_High_Count_Fails()
        {
            Client.Pylon.Sample(VALID_ID, 101);
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Sample_Too_Late_Start_Fails()
        {
            Client.Pylon.Sample(VALID_ID, start: DateTimeOffset.Now.AddDays(1));
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Sample_Too_Late_End_Fails()
        {
            Client.Pylon.Sample(VALID_ID, end: DateTimeOffset.Now.AddDays(1));
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Sample_End_Before_Start_Fails()
        {
            Client.Pylon.Sample(VALID_ID, start: VALID_START, end: DateTimeOffset.Now.AddDays(-31));
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Sample_Empty_Filter_Fails()
        {
            Client.Pylon.Sample(VALID_ID, filter: "");
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Sample_Succeeds()
        {
            var response = Client.Pylon.Sample(VALID_ID);
            Assert.AreEqual(80, response.Data.remaining);
            Assert.AreEqual("en", response.Data.interactions[0].fb.language);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            throw new NotImplementedException();
        }

        #endregion

    }
}
