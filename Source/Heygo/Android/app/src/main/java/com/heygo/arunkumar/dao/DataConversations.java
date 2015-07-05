
package com.heygo.arunkumar.dao;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

import com.heygo.arunkumar.data.Contact;
import com.heygo.arunkumar.data.Conversation;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Date;

/**
 * Created by Arunkumar on 5/22/2015.
 */
public class DataConversations {
    public static final String TAG = "DataConversations";

    private DataHandler mDataHandler;
    private SQLiteDatabase mSQLiteDatabase;
    private Context mContext;
    private String[] mAllColumns = new String[]{
            DataHandler.COLUMN_CONVERSATIONID,
            DataHandler.COLUMN_CONVERSATIONMESSAGE,
            DataHandler.COLUMN_CONVERSATIONOWNERID,
            DataHandler.COLUMN_CONVERSATIONTOGROUPID,
            DataHandler.COLUMN_CONVERSATIONTOCONTACTID,
            DataHandler.COLUMN_CONVERSATIONDATETIME,
            DataHandler.COLUMN_CONVERSATIONISACTIVE
    };

    public DataConversations(Context context) {
        mContext = context;
        mDataHandler = new DataHandler(context);
        try {
            Open();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public void Open() throws SQLException {
        mSQLiteDatabase = mDataHandler.getWritableDatabase();
    }

    public void Close() {
        mDataHandler.close();
    }

    public long CreateConversation(Conversation conversation) {
        ContentValues contentValues = new ContentValues();
        contentValues.put(DataHandler.COLUMN_CONVERSATIONMESSAGE, conversation.Message);
        contentValues.put(DataHandler.COLUMN_CONVERSATIONOWNERID, conversation.OwnerID);
        contentValues.put(DataHandler.COLUMN_CONVERSATIONTOGROUPID, conversation.ToGroupID);
        contentValues.put(DataHandler.COLUMN_CONVERSATIONTOCONTACTID, conversation.ToContactID);
        return mSQLiteDatabase.insert(DataHandler.TABLE_CONVERSATION, null, contentValues);
    }

    public Cursor GetAllActiveContacts() {
        return mSQLiteDatabase.query(DataHandler.TABLE_CONVERSATION, null, null, null, null, null, null);
    }

    public Cursor GetConversationsById(int contactId) {
        return mSQLiteDatabase.query(DataHandler.TABLE_CONVERSATION, mAllColumns,
                DataHandler.COLUMN_CONVERSATIONOWNERID + "=" + contactId, null, null, null, null);

        /*if (cursor == null)
            return null;

        if (cursor.moveToFirst()) {
            ArrayList<Conversation> conversations = new ArrayList<>();
            do {
                Conversation data = new Conversation();
                data.Message = cursor.getString(1);
                data.Created = new Date(cursor.getString(5));
                conversations.add(data);
            } while (cursor.moveToNext());
            return conversations;
        }
        return null;*/
    }
}
