package com.heygo.arunkumar.dao;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

import com.heygo.arunkumar.data.Contact;

import java.sql.SQLException;

/**
 * Created by Arunkumar on 5/22/2015.
 */
public class DataContacts {
    public static final String TAG = "DataContacts";

    private DataHandler mDataHandler;
    private SQLiteDatabase mSQLiteDatabase;
    private Context mContext;
    private String[] mAllColumns = new String[]{
            DataHandler.COLUMN_CONTACTID,
            DataHandler.COLUMN_CONTACTNAME,
            DataHandler.COLUMN_CONTACTPHONE,
            DataHandler.COLUMN_CONTACTREGID,
            DataHandler.COLUMN_CONTACTCREATED,
            DataHandler.COLUMN_CONTACTUPDATED,
            DataHandler.COLUMN_CONTACTISACTIVE
    };

    public DataContacts(Context context){
        mContext = context;
        mDataHandler = new DataHandler(context);
        try {
            Open();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public void Open() throws SQLException
    {
        mSQLiteDatabase = mDataHandler.getWritableDatabase();
    }

    public void Close(){
        mDataHandler.close();
    }

    public long CreateContact(Contact contact){
        ContentValues contentValues = new ContentValues();
        contentValues.put(DataHandler.COLUMN_CONTACTNAME, contact.Name);
        contentValues.put(DataHandler.COLUMN_CONTACTPHONE, contact.PhoneNumber);
        contentValues.put(DataHandler.COLUMN_CONTACTREGID, contact.RegisterKey);
        return mSQLiteDatabase.insert(DataHandler.TABLE_CONTACT, null, contentValues);
    }

    public Cursor GetAllActiveContacts(){
        return mSQLiteDatabase.query(DataHandler.TABLE_CONTACT, null, null, null, null, null, null);
    }

    public Contact GetContact(String phone)
    {
        Cursor cursor = mSQLiteDatabase.query(DataHandler.TABLE_CONTACT, mAllColumns,
                                    DataHandler.COLUMN_CONTACTPHONE + "='" + phone + "'", null, null, null, null);

        if(cursor == null)
            return null;

        cursor.moveToFirst();
        Contact contact = new Contact();
        contact.ID = cursor.getInt(0);
        contact.Name = cursor.getString(1);
        contact.PhoneNumber = cursor.getString(2);
        return contact;
    }
}
