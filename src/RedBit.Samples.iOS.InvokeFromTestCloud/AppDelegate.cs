using System;

using Foundation;
using UIKit;

namespace RedBit.Samples.iOS.InvokeFromTestCloud
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.

	[Register ("AppDelegate")]
	[Protocol]
	public partial class AppDelegate : UIApplicationDelegate
	{
		
		[Export("InvokeAddNewItem:"), ]
		public void InvokeAddNewItem(string val){
			MasterViewController.InvokeAddNewItem ();
		}

		[Export("InvokeTapItem:")]
		public void InvokeTapItem(string index){
			MasterViewController.InvokeTapItem (index);
		}

		// class-level declarations

		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			#if DEBUG
			Xamarin.Calabash.Start();
			#endif

			// Override point for customization after application launch.
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				var splitViewController = (UISplitViewController)Window.RootViewController;

				// Get the UINavigationControllers containing our master & detail view controllers
				var masterNavigationController = (UINavigationController)splitViewController.ViewControllers [0];
				var detailNavigationController = (UINavigationController)splitViewController.ViewControllers [1];

				var masterViewController = (MasterViewController)masterNavigationController.TopViewController;
				var detailViewController = (DetailViewController)detailNavigationController.TopViewController;

				masterViewController.DetailViewController = detailViewController;

				// Set the DetailViewController as the UISplitViewController Delegate.
				splitViewController.WeakDelegate = detailViewController;
			}

			return true;
		}

		//
		// This method is invoked when the application is about to move from active to inactive state.
		//
		// OpenGL applications should use this method to pause.
		//
		public override void OnResignActivation (UIApplication application)
		{
		}

		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background exection this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
		}

		// This method is called as part of the transiton from background to active state.
		public override void WillEnterForeground (UIApplication application)
		{
		}

		// This method is called when the application is about to terminate. Save data, if needed.
		public override void WillTerminate (UIApplication application)
		{
		}
	}
}

