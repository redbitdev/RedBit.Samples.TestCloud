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

        Func<AppQuery, AppQuery> AddButton { get; }

        Func<AppQuery, AppQuery> ListTextItem { get; }
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

        public Func<AppQuery, AppQuery> AddButton
        {
            get
            {
                return c => c.Button("Add");
            }
        }

        public Func<AppQuery, AppQuery> ListTextItem
        {
            get
            {
                return s => s.Class("UITableViewLabel");
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

        public Func<AppQuery, AppQuery> AddButton
        {
            get
            {
                return c => c.Marked("action_add");
            }
        }

        public Func<AppQuery, AppQuery> ListTextItem
        {
            get
            {
                return s => s.Class("TextView");
            }
        }
        #endregion
    }
}

