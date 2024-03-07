using NUnit.Framework;
using UnityEngine;

namespace Duck.Http.Tests.PlayMode
{
	[TestFixture]
	public class HttpTests
	{
		private Http http;

		[OneTimeSetUp]
		public void Setup()
		{
			http = Http.Instance;
		}

		[OneTimeTearDown]
		public void TearDown()
		{
			Object.DestroyImmediate(http.gameObject);
		}

		[Test]
		public void Expect_Singleton_Is_Created()
		{
			Assert.IsNotNull(http);
		}

		[Test]
		public void Expect_Set_Super_Headers()
		{
			const string HEADER_KEY = "HeaderKey";
			const string HEADER_VALUE = "HeaderValue";

			Http.SetSuperHeader(HEADER_KEY, HEADER_VALUE);
			var headers = Http.GetSuperHeaders();

			Assert.IsTrue(headers.ContainsKey(HEADER_KEY));
			Assert.AreEqual(HEADER_VALUE, headers[HEADER_KEY]);
			Assert.AreEqual(1, headers.Count);
		}
		
		[Test]
		public void Put_Save_On_Server()
		{
			const string URI = "https://file.io/";
			const string FileGetURI = URI + "TESTPROJECT123";

			string responseText1;
			string responseText2;
			string saveDataPath = "/PlayerData.json";
			var requestPut = Http.Put(FileGetURI, System.IO.File.ReadAllText(Application.persistentDataPath + saveDataPath))
				.SetHeader("accept", "application/json")
				.SetHeader("Authorization", "Bearer VP6YMM4.R9WGASK-GFVM6VZ-N9MXDFJ-E960J2Y")
				.SetHeader("Content-Type", "multipart/form-data")
				.OnSuccess(response => {
					Debug.Log(response.Text);
					responseText1 = response.Text;
				})
				.OnError(response => Debug.Log(response.StatusCode))
				.OnDownloadProgress(progress => Debug.Log(progress))
				.Send();
			// var requestGet = Http.Get(FileGetURI)
			// 	.SetHeader("accept", "application/json")
			// 	.SetHeader("Authorization", "Bearer VP6YMM4.R9WGASK-GFVM6VZ-N9MXDFJ-E960J2Y")
			// 	.OnSuccess(response => {
			// 		Debug.Log(response.Text);
			// 		responseText2 = response.Text;
			// 	})
			// 	.OnError(response => Debug.Log(response.StatusCode))
			// 	.OnDownloadProgress(progress => Debug.Log(progress))
			// 	.Send();
			//
			// Assert.AreEqual(responseText1, responseText2);
		}
	}
}
