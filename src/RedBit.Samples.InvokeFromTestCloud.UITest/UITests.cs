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
        private IScreenQueries _screenQueries;
        private bool _isIos = false;

		public string PathToIPA { get; private set; }

        public string PathToAPK { get; private set; }

        private string API_KEY = "8cf2e1b42aa48fad1944279044555025";


		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			if (TestEnvironment.IsTestCloud)
			{
                PathToAPK = String.Empty;
                PathToIPA = String.Empty;
			}
			else
			{

				string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
				FileInfo fi = new FileInfo(currentFile);
				string dir = fi.Directory.Parent.Parent.Parent.FullName;

				PathToIPA = Path.Combine(dir, "RedBit.Samples.iOS.InvokeFromTestCloud", "bin", "iPhoneSimulator", "Debug", "RedBit.Samples.iOS.InvokeFromTestCloud.app");
                PathToAPK = Path.Combine(dir, "RedBit.Samples.Droid.InvokeFromTestCloud", "bin", "Release", "RedBit.Samples.Droid.InvokeFromTestCloud.apk");
            }
		}
			
		[SetUp]
		public void SetUp()
		{
			if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudiOS))
			{
                // Setup the screenQueries
                _screenQueries = new iOSScreenQueries();

                _isIos = true;

                // configure the app
				_app = ConfigureApp
					.iOS
					.StartApp();
			}
			else if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudAndroid))
			{
                // Setup the screenQueries
                _screenQueries = new AndroidScreenQueries();

                // configure the app
                _app = ConfigureApp
                    .Android
                    .StartApp();
			}
			else if (TestEnvironment.Platform.Equals(TestPlatform.Local))
			{
				// NOTE Enable or disable the lines depending on what platform you want ot test
//				SetupForiOSLocalTesting ();				
                SetupForAndroidLocalTesting();
			}
			else
			{
				throw new NotImplementedException(String.Format("I don't know this platform {0}", TestEnvironment.Platform));
			}
		}

		private void SetupForiOSLocalTesting()
		{
            _isIos = true;

            // Setup the screenQueries
            _screenQueries = new iOSScreenQueries();
            
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

        private void SetupForAndroidLocalTesting(){
            // Setup the screenQueries
            _screenQueries = new AndroidScreenQueries();

            // configure the android appuitest 
            _app = ConfigureApp.Android
                .ApiKey(API_KEY)
                .ApkFile(PathToAPK)
                .StartApp();
        }

        /// <summary>
        /// This is how a real test should be written and not using an export attribute. 
        /// The samples are only to get started with exported 
        /// </summary>
        [Test]
        public void TapItemTest(){
            _app.WaitForElement(_screenQueries.MainControl);

            // tap the add button
            _app.Tap(_screenQueries.AddButton);

            // what for the item
            _app.WaitForElement(_screenQueries.ListTextItem);

            // select the item in tableview
            _app.Tap(_screenQueries.ListTextItem);

            if (_isIos)
            {
                // for ios we go back
                _app.Back();
            }
            else
            {
                // for android we need to make sure there is a message
                _app.WaitForElement(s => s.Marked("message"));
            }

            // assert as we are good
            Assert.Pass();

        }

        [Test]
        public void TapItemTestWithInvoke()
        {
            // wait for nav button so we know splash is gone
            _app.WaitForElement(_screenQueries.MainControl);

            // add an item
            _app.Invoke("InvokeAddNewItem", "");

            // what for the item
            _app.WaitForElement(_screenQueries.ListTextItem);

            // tap the first item
            _app.Invoke("InvokeTapItem", "0");

            if (_isIos)
            {
                // for ios we go back
                _app.Back();
            }
            else
            {
                // for android we need to make sure there is a message
                _app.WaitForElement(s => s.Marked("message"));
            }

            // assert as we are good
            Assert.Pass();
        }

		[Test]
		public void InvokeAddNewItemTest(){
			// wait for nav button so we know splash is gone
            _app.WaitForElement(_screenQueries.MainControl);

			// invoke the method within masterviewcontroller
			_app.Invoke("InvokeAddNewItem", "");

		}

		[Test]
		public void InvokeTapItemTest(){
			// wait for nav button so we know splash is gone
            _app.WaitForElement(_screenQueries.MainControl);

			// add an item
			_app.Invoke("InvokeAddNewItem", "");

			// tap the first item
			_app.Invoke("InvokeTapItem", "0");
		}

		

		[Test]
		public void InvokeTapItemTestFail(){
			// wait for nav button so we know splash is gone
            _app.WaitForElement(_screenQueries.MainControl);

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

