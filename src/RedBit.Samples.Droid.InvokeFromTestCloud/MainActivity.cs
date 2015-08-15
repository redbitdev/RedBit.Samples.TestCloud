using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedBit.Samples.Droid.InvokeFromTestCloud
{
    [Activity(Label = "RedBit.Samples.Droid.InvokeFromTestCloud", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ListActivity
    {
        private DTAdapter _adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _adapter = new DTAdapter(this);

            this.ListAdapter = _adapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add:
                    InvokeAddNewItem(null);
                    break;
                default:
                    break;
            }

            return true;
        }
        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var t = _adapter.GetItemText(position);
            Android.Widget.Toast.MakeText(this, t, Android.Widget.ToastLength.Long).Show();
        }

        void AddNewItem ()
        {
            _adapter.AddItem(DateTime.Now.ToString());
            _adapter.NotifyDataSetChanged();
        }

        [Preserve]
        [Java.Interop.Export]
        public void InvokeAddNewItem(string value){
            AddNewItem();
        }

        [Preserve]
        [Java.Interop.Export]
        public void InvokeTapItem(string index){
            var val = default(Int32);
            if (Int32.TryParse(index, out val))
            {
                this.ListView.PerformItemClick(
                    this.ListView.Adapter.GetView(val, null, null),
                    val,
                    this.ListView.Adapter.GetItemId(val)
                );
            }
            else
                throw new Exception(string.Format("Unable to parse index parameter of '{0}'", index)); 
        }
    }

    public class DTAdapter : BaseAdapter<string>
    {
        private List<string> _items;
        private Activity _context;

        public DTAdapter(Activity context) : base() {
            this._context = context;
            this._items = new List<string>();
        }

        public string GetItemText(int position)
        {
            return _items[position];
        }

        public void AddItem(string item)
        {
            _items.Add(item);
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position] {  
            get { return _items[position]; }
        }
        public override int Count {
            get { return _items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = _items[position];
            return view;
        }
    }
}


