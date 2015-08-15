using NUnit.Framework;
using System;
using Xamarin.UITest;
using System.Reflection;
using System.IO;
using Xamarin.UITest.Queries;
using System.Linq;
using System.Text;
using System.Threading;

namespace RedBit
{
	public class UITests
	{
		internal IApp _app;

		public string PathToIPA { get; private set; }

		private string API_KEY = "8cf2e1b42aa48fad1944279044555025";

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			if (TestEnvironment.IsTestCloud)
			{
				PathToIPA = String.Empty;
			}
			else
			{

				string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
				FileInfo fi = new FileInfo(currentFile);
				string dir = fi.Directory.Parent.Parent.Parent.FullName;

				PathToIPA = Path.Combine(dir, "RedBit.Samples.iOS.InvokeFromTestCloud", "bin", "iPhoneSimulator", "Debug", "RedBit.Samples.iOS.InvokeFromTestCloud.app");
			}
		}
			
		[SetUp]
		public void SetUp()
		{
			if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudiOS))
			{// configure the app
				_app = ConfigureApp
					.iOS
					.StartApp();
			}
			else if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudAndroid))
			{
				// no op
			}
			else if (TestEnvironment.Platform.Equals(TestPlatform.Local))
			{

				// NOTE Enable or disable the lines depending on what platform you want ot test
				SetupForiOSLocalTesting ();				
				    
			}
			else
			{
				throw new NotImplementedException(String.Format("I don't know this platform {0}", TestEnvironment.Platform));
			}
		}

		private void SetupForiOSLocalTesting()
		{
			// NOTE - device identifier is different on every machine so need to run this
			// on the command line "xcrun instruments -s devices" to get a list of devices

			// configure the ios app
			//				_app = ConfigureApp.iOS
			//					.ApiKey("009518f1f4759a83d3a34b3bfe921438")
			//					.DeviceIdentifier ("8fca3b019aea68dec6b5e2bfedad2298c5f8af3a") // marks ipod
			//					.InstalledApp (PathToIPA)
			//					.StartApp ();

			_app = ConfigureApp.iOS
				.ApiKey(API_KEY)
				.AppBundle(PathToIPA)
				.StartApp();

		}

		[Test]
		public void InvokeAddNewItemTest(){

            _app.WaitFor(500);

			// wait for nav button so we know splash is gone
			_app.WaitForElement(s=>s.Class("UINavigationButton"));

			// invoke the method within masterviewcontroller
			_app.Invoke("InvokeAddNewItem:", "");

		}


		[Test]
		public void InvokeTapItemTest(){
            _app.WaitFor(500);

			// wait for nav button so we know splash is gone
			_app.WaitForElement(s=>s.Class("UINavigationButton"));
//			_app.Repl ();
			// add an item
			_app.Invoke("InvokeAddNewItem", "");

			// tap the first item
			_app.Invoke("InvokeTapItem", "0");
		}

		/// <summary>
		/// This is how a real test should be written and not using an export attribute. The samples are only to get started with exported 
		/// </summary>
		[Test]
		public void TapItemTest(){
            _app.WaitFor(500);

			_app.WaitForElement(s=>s.Class("UINavigationButton"));

			// tap the add button
			_app.Tap (c => c.Button ("Add"));

			// what for the item
			_app.WaitForElement(s=>s.Class("UITableViewLabel"));

			// select the item in tableview
			_app.Tap(s=>s.Class("UITableViewLabel"))   ;

			// go back
			_app.Back();

			// assert as we are good
			Assert.Pass();
		}

		[Test]
		public void InvokeTapItemTestFail(){
            _app.WaitFor(500);

			// wait for nav button so we know splash is gone
			_app.WaitForElement (s => s.Class ("UINavigationButton"));

			// add an item
			_app.Invoke ("InvokeAddNewItem", "");

			try {
				// send a param that will not parse so the app will throw an exception
				_app.Invoke ("InvokeTapItem", "t");
				Assert.Fail ("This should have thrown an exception");
			} catch (Exception) {
				Assert.Pass ();
			}
		}
	}
}

