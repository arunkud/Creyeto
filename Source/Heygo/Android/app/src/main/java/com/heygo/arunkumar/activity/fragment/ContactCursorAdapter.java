package com.heygo.arunkumar.activity.fragment;

import android.content.Context;
import android.database.Cursor;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CursorAdapter;
import android.widget.Filterable;
import android.widget.TextView;

import com.example.arunkumar.test10.R;
import com.heygo.arunkumar.common.Shared;
import com.heygo.arunkumar.dao.DataHandler;

import java.util.logging.Filter;

/**
 * Created by Arunkumar on 5/24/2015.
 */
public class ContactCursorAdapter extends CursorAdapter implements Filterable {

    private LayoutInflater mInflater;

    public ContactCursorAdapter(Context context, Cursor c) {
        super(context, c, true);
        mInflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

    @Override
    public void bindView(View view, Context context, Cursor cursor) {

        TextView content = (TextView) view.findViewById(R.id.Name);
        TextView phone = (TextView) view.findViewById(R.id.phoneNumber);
        content.setTag(R.string.tag_contactid, cursor.getInt(0));
        content.setText(cursor.getString(1));
        phone.setText(cursor.getString(2));
        content.setTag(R.string.tag_regkey, cursor.getString(3));

    }

//    public Filter getFilter() {
//        return new Filter() {
//
//            @Override
//            protected android.widget.Filter.FilterResults performFiltering(CharSequence constraint) {
//                final android.widget.Filter.FilterResults oReturn = new android.widget.Filter.FilterResults();
//                final ArrayList<Employee> results = new ArrayList<Employee>();
//                if (orig == null)
//                    orig = employeeArrayList;
//                if (constraint != null) {
//                    if (orig != null && orig.size() > 0) {
//                        for (final Employee g : orig) {
//                            if (g.getName().toLowerCase()
//                                    .contains(constraint.toString()))
//                                results.add(g);
//                        }
//                    }
//                    oReturn.values = results;
//                }
//                return oReturn;
//            }
//
//            @SuppressWarnings("unchecked")
//            @Override
//            protected void publishResults(CharSequence constraint,
//                                          FilterResults results) {
//                employeeArrayList = (ArrayList<Employee>) results.values;
//                notifyDataSetChanged();
//            }
//        };
//    }
//
//    public void notifyDataSetChanged() {
//        super.notifyDataSetChanged();
//    }


    @Override
    public View newView(Context context, Cursor cursor, ViewGroup parent) {
        return mInflater.inflate(R.layout.contact_item, parent, false);
    }

}