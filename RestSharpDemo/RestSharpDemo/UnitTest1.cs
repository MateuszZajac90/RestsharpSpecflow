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
			request.AddBody(new { name = "Kowalski" });

			request.AddUrlSegment("postid", 1);

			var response = client.Execute(request);

			var result = response.DeserializeResponse()["name"];

			//JObject obs = JObject.Parse(response.Content);

			Assert.That(result, Is.EqualTo("Kowalski"), "Author is not correct.");

		}

		[Test]
		public void T3PostWithGenericBody()
		{
			var client = new RestClient("http://localhost:3000/");

			var request = new RestRequest("posts", Method.POST);

			request.RequestFormat = DataFormat.Json;
			request.AddBody(new Posts() { id ="27", author="Nowakowski", title="C# versus Java" });

			var response = client.Execute(request);

			var deserialize = new JsonDeserializer();
			var output = deserialize.Deserialize<Dictionary<string, string>>(response);
			var result = output["author"];

			//JObject obs = JObject.Parse(response.Content);

			Assert.That(result, Is.EqualTo("Nowakowski"), "Author is not correct.");

		}

		[Test]
		public void T4GenericDeserialize()
		{
			var client = new RestClient("http://localhost:3000/");

			var request = new RestRequest("posts", Method.POST);

			request.RequestFormat = DataFormat.Json;
			request.AddBody(new Posts() { id = "26", author = "Nowakowski", title = "C# versus Java" });

			var response = client.Execute<Posts>(request);

			Assert.That(response.Data.author, Is.EqualTo("Nowakowski"), "Author is not correct.");

		}

		[Test]
		public  void T5Async()
		{
			var client = new RestClient("http://localhost:3000/");

			var request = new RestRequest("posts", Method.POST);

			request.RequestFormat = DataFormat.Json;
			request.AddBody(new Posts() { id = "30", author = "Nowakowski", title = "C# versus Java" });

			//var response = client.Execute<Posts>(request);

			//LIB 3 - using async
			var response =  client.ExecuteAsyncRequest<Posts>(request).GetAwaiter().GetResult();

			Assert.That(response.Data.author, Is.EqualTo("Nowakowski"), "Author is not correct.");

		}

		


	}
}
