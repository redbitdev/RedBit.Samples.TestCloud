using System;
using Xamarin.UITest;
using System.Threading;
using Xamarin.UITest.Queries;
using System.Linq;

namespace RedBit
{
	public static class AppExtension
	{
		public static void WaitFor(	this IApp app, TimeSpan time){
			var waitUntil = DateTime.Now.Add (time);
			while (waitUntil.Ticks > DateTime.Now.Ticks)
				Thread.Sleep (100);
		}

		public static void WaitFor(	this IApp app, int timeInSeconds){
			app.WaitFor(TimeSpan.FromSeconds(timeInSeconds));
		}

		public static void WaitFor(this IApp app, double timeInMilliSeconds = 250){
			app.WaitFor (TimeSpan.FromMilliseconds (timeInMilliSeconds));
		}


	}
}

