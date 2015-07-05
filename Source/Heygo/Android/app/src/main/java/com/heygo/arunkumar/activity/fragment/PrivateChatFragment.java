package com.heygo.arunkumar.activity.fragment;

import android.content.Intent;
import android.database.Cursor;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListView;
import android.widget.SearchView;
import android.widget.TextView;

import com.example.arunkumar.test10.R;
import com.heygo.arunkumar.activity.FindContacts;
import com.heygo.arunkumar.activity.PrivateOneToOne;
import com.heygo.arunkumar.common.Shared;
import com.heygo.arunkumar.dao.DataContacts;

/**
 * Created by Arunkumar on 5/16/2015.
 */
public class PrivateChatFragment extends Fragment  implements SearchView.OnQueryTextListener {
    ListView lstContacts;
    SearchView mSearchView;
    MenuItem searchMenuBar;
    Button btnAddContact;
    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.private_chats, container, false);
        lstContacts = (ListView)view.findViewById(R.id.lstContacts);
        mSearchView=(SearchView) view.findViewById(R.id.searchView);
        lstContacts.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                Intent intent = new Intent(view.getContext(), PrivateOneToOne.class);
                TextView txtName = (TextView) view.findViewById(R.id.Name);
                TextView txtPhone = (TextView) view.findViewById(R.id.phoneNumber);
                intent.putExtra("ContactId", txtName.getTag(R.string.tag_contactid).toString());
                intent.putExtra("Name", txtName.getText().toString());
                intent.putExtra("Phone", txtPhone.getText().toString());
                intent.putExtra(Shared.CONST_GOOGLE_PROPERTY_REG_ID, txtName.getTag(R.string.tag_regkey).toString());
                startActivity(intent);
            }
        });
        PopulateContactsList(view);
        setupSearchView();
        setHasOptionsMenu(true);

        return view;
    }

    private void setupSearchView()
    {
        mSearchView.setIconifiedByDefault(false);
        mSearchView.setOnQueryTextListener(this);
        mSearchView.setSubmitButtonEnabled(true);
        mSearchView.setQueryHint("Search Here");
    }

    @Override
    public boolean onQueryTextSubmit(String query) {
        return false;
    }

    @Override
    public boolean onQueryTextChange(String newText)
    {
        if (TextUtils.isEmpty(newText.toString())) {
            lstContacts.clearTextFilter();
        } else {
            lstContacts.setFilterText(newText.toString());
        }
        return true;
    }


    private void PopulateContactsList(View view){
        DataContacts dtContacts = new DataContacts(view.getContext());

        Cursor cursor = dtContacts.GetAllActiveContacts();
        try {
            ContactCursorAdapter contactCursorAdapter = new ContactCursorAdapter(view.getContext(), cursor);
            lstContacts.setAdapter(contactCursorAdapter);
            lstContacts.setTextFilterEnabled(true);
        }
        catch (Exception ex){
            Log.e("ERROR", ex.getMessage());
        }
    }
}
