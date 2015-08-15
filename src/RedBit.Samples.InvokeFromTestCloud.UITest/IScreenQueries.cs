using System;
using Xamarin.UITest.Queries;

namespace RedBit
{
    public interface IScreenQueries
    {
        /// <summary>
        /// Main control to wait for in tests
        /// </summary>
        /// <value>The main control.</value>
        Func<AppQuery, AppQuery> MainControl { get; }
    }

    public class iOSScreenQueries : IScreenQueries
    {
        #region IScreenQueries implementation
        public Func<AppQuery, AppQuery> MainControl
        {
            get
            {
                return c => c.Button("Add");
            }
        }
        #endregion
    }

    public class AndroidScreenQueries : IScreenQueries
    {
        #region IScreenQueries implementation
        public Func<AppQuery, AppQuery> MainControl
        {
            get
            {
                return c => c.Class("ListView");
            }
        }
        #endregion
    }
}

