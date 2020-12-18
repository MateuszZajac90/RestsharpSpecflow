using System.Collections.Generic;
using RestSharp;
using RestSharp.Deserializers;
using NUnit.Framework;
using RestSharp.Serialization.Json;
using Newtonsoft.Json.Linq;
using RestSharpDemo.Model;
using System.Threading.Tasks;
using System;
using RestSharpDemo.Utilities;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System.IO;

namespace RestSharpDemo
{
	[TestFixture]
	public class UnitTest1
	{
		[Test]
		public void T1SimpleGet()
		{
			var client = new RestClient("http://localhost:3000/");

			var request = new RestRequest("posts/{postid}", Method.GET);

			request.AddUrlSegment("postid", 1);
			 
			var response = client.Execute(request);

			//LIB 1 - Dictionary based response
			//var deserialize = new JsonDeserializer();
			//var output = deserialize.Deserialize<Dictionary<string, string>>(response);
			//var result = output["author"];

			//LIB 2 - JSON based response
			JObject obs = JObject.Parse(response.Content);
			Assert.That(obs["author"].ToString(), Is.EqualTo("Mateusz"), "Author is not correct.");

		}

		[Test]
		public void T2SimplePost()
		{
			var client = new RestClient("http://localhost:3000/");

			var request = new RestRequest("posts/{postid}/profile", Method.POST);

			request.RequestFormat = DataFormat.Json;
			request.AddBody(new { name = "Nowak" });

			request.AddUrlSegment("postid", 2);

			var response = client.Execute(request);

			var result = response.DeserializeResponse()["name"];

			//JObject obs = JObject.Parse(response.Content);

			Assert.That(result, Is.EqualTo("Nowak"), "Author is not correct.");

		}

		[Test]
		public void T3PostWithGenericBody()
		{
			var client = new RestClient("http://localhost:3000/");

			var request = new RestRequest("posts", Method.POST);

			request.RequestFormat = DataFormat.Json;
			request.AddBody(new Posts() { id ="7", author="Sienkiewicz", title="XXX" });

			var response = client.Execute(request);

			var deserialize = new JsonDeserializer();
			var output = deserialize.Deserialize<Dictionary<string, string>>(response);
			var result = output["author"];

			//JObject obs = JObject.Parse(response.Content);

			Assert.That(result, Is.EqualTo("Sienkiewicz"), "Author is not correct.");

		}

		[Test]
		public void T4GenericDeserialize()
		{
			var client = new RestClient("http://localhost:3000/");

			var request = new RestRequest("posts", Method.POST);

			request.RequestFormat = DataFormat.Json;
			request.AddBody(new Posts() { id = "8", author = "Mickiewicz", title = "YYY" });

			var response = client.Execute<Posts>(request);

			Assert.That(response.Data.author, Is.EqualTo("Mickiewicz"), "Author is not correct.");

		}

		[Test]
		public  void T5Async()
		{
			var client = new RestClient("http://localhost:3000/");

			var request = new RestRequest("posts", Method.POST);

			request.RequestFormat = DataFormat.Json;
			request.AddBody(new Posts() { id = "10", author = "Rowling", title = "ZZZ" });

			//var response = client.Execute<Posts>(request);

			//LIB 3 - using async
			var response =  client.ExecuteAsyncRequest<Posts>(request).GetAwaiter().GetResult();

			Assert.That(response.Data.author, Is.EqualTo("Rowling"), "Author is not correct.");

		}

		[Test]
		public void AuthenticationMechanism()
		{
			var client = new RestClient("http://localhost:3000/");

			var request = new RestRequest("auth/login", Method.POST);

			request.RequestFormat = DataFormat.Json;
			request.AddBody(new { email = "nilson@email.com", password = "nilson" });

			var response = client.ExecutePostTaskAsync(request).GetAwaiter().GetResult();
			var access_token = response.DeserializeResponse()["access_token"];

			var jwtAuth = new JwtAuthenticator(access_token);
			client.Authenticator = jwtAuth;

			var getRequest = new RestRequest("posts/{postid}", Method.GET);
			getRequest.AddUrlSegment("postid", 5);
			var result = client.ExecuteAsyncRequest<Posts>(getRequest).GetAwaiter().GetResult();
			Assert.That(result.Data.author, Is.EqualTo("Sienkiewicz"), "The author is incorrect");

		}

		[Test]
		public void AuthenticationMechanismWithJSONFile()
		{
			var client = new RestClient("http://localhost:3000/");

			var request = new RestRequest("auth/login", Method.POST);
			var file = @"TestData\Data.json";

			request.RequestFormat = DataFormat.Json;
			var jsonData = JsonConvert.DeserializeObject<User>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file)).ToString());
			request.AddJsonBody(jsonData);

			var response = client.ExecutePostTaskAsync(request).GetAwaiter().GetResult();
			var access_token = response.DeserializeResponse()["access_token"];

			var jwtAuth = new JwtAuthenticator(access_token);
			client.Authenticator = jwtAuth;

			var getRequest = new RestRequest("posts/{postid}", Method.GET);
			getRequest.AddUrlSegment("postid", 5);
			var result = client.ExecuteAsyncRequest<Posts>(getRequest).GetAwaiter().GetResult();
			Assert.That(result.Data.author, Is.EqualTo("Sienkiewicz"), "The author is incorrect");

		}

        private class User
        {
			public string email { get; set; }
			public string password { get; set; }

		}
	}
}
