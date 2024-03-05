using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Duck.Http.Tests.EditMode
{
	[TestFixture]
	public class UtilsTests
	{
		[Test]
		public void Expect_Construct_Uri_From_Parameters()
		{
			const string URI = "https://subdomain.domain.extension/index.html";
			const string EXPECTED_CONSTRUCTED_URI = URI + "?page=2&start=25&count=5";
			var parameters = new Dictionary<string, string>
			{
				{"page", "2"},
				{"start", "25"},
				{"count", "5"}
			};
			var constructUriWithParameters = HttpUtils.ConstructUriWithParameters(URI, parameters);
			Assert.AreEqual(EXPECTED_CONSTRUCTED_URI, constructUriWithParameters);
		}
		
		[Test]
		public void Put_Save_On_Server()
		{
			const string URI = "https://file.io/";
			const string FileGetURI = URI + "TESTPROJECT123";
			
			string saveDataPath = "/PlayerData.json";
			var requestPut = Http.Put(FileGetURI, System.IO.File.ReadAllText(Application.persistentDataPath + saveDataPath))
				.SetHeader("accept", "application/json")
				.SetHeader("Authorization", "Bearer VP6YMM4.R9WGASK-GFVM6VZ-N9MXDFJ-E960J2Y")
				.SetHeader("Content-Type", "multipart/form-data")
				.OnSuccess(response => Debug.Log(response.Text))
				.OnError(response => Debug.Log(response.StatusCode))
				.OnDownloadProgress(progress => Debug.Log(progress))
				.Send();
			var requestGet = Http.Get(FileGetURI)
				.SetHeader("accept", "application/json")
				.SetHeader("Authorization", "Bearer VP6YMM4.R9WGASK-GFVM6VZ-N9MXDFJ-E960J2Y")
				.OnSuccess(response => Debug.Log(response.Text))
				.OnError(response => Debug.Log(response.StatusCode))
				.OnDownloadProgress(progress => Debug.Log(progress))
				.Send();
			
			// Assert.AreEqual(FileGetURI, constructUriWithParameters);
		}
	}
}
