package com.heygo.arunkumar.dao;

import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.util.Log;
import android.widget.Toast;

/**
 * Created by Arunkumar on 5/20/2015.
 */
public class DataHandler extends SQLiteOpenHelper {

    public static final String DATABASE_NAME = "HeyGoDb";
    public static final int DATABASE_VERSION = 2;

    public static final String TABLE_GROUP = "UserGroup";
    public static final String COLUMN_GROUPID = "_id";
    public static final String COLUMN_GROUPNAME = "Name";
    public static final String COLUMN_CREATED = "Created";
    public static final String COLUMN_UPDATED = "Updated";
    public static final String COLUMN_ISACTIVE = "IsActive";

    public static final String TABLE_CONTACT = "Contact";
    public static final String COLUMN_CONTACTID = "_id";
    public static final String COLUMN_CONTACTNAME = "Name";
    public static final String COLUMN_CONTACTPHONE = "Phone";
    public static final String COLUMN_CONTACTREGID = "RegistrationId";
    public static final String COLUMN_CONTACTCREATED = "Created";
    public static final String COLUMN_CONTACTUPDATED = "Updated";
    public static final String COLUMN_CONTACTISACTIVE = "IsActive";

    public static final String TABLE_CONTACTGROUP = "ContactGroup";
    public static final String COLUMN_CONTACTGROUPID = "_id";
    public static final String COLUMN_CONTACTGROUPCONTACTID = "ContactID";
    public static final String COLUMN_CONTACTGROUPGROUPID = "GroupID";
    public static final String COLUMN_CONTACTGROUPCREATED = "Created";
    public static final String COLUMN_CONTACTGROUPUPDATED = "Updated";
    public static final String COLUMN_CONTACTGROUPISACTIVE = "IsActive";

    public static final String TABLE_CONVERSATION = "Conversation";
    public static final String COLUMN_CONVERSATIONID = "_id";
    public static final String COLUMN_CONVERSATIONMESSAGE = "Message";
    public static final String COLUMN_CONVERSATIONOWNERID = "OwnerID";
    public static final String COLUMN_CONVERSATIONTOGROUPID = "ToGroupID";
    public static final String COLUMN_CONVERSATIONTOCONTACTID = "ToContactID";
    public static final String COLUMN_CONVERSATIONDATETIME = "Created";
    public static final String COLUMN_CONVERSATIONISACTIVE = "IsActive";

    public static final String QUERY_CREATE_GROUP = "CREATE TABLE IF NOT EXISTS " + TABLE_GROUP + " (" +
            COLUMN_GROUPID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
            COLUMN_GROUPNAME + " TEXT NOT NULL, " +
            COLUMN_CREATED + " TIMESTAMP DEFAULT CURRENT_TIMESTAMP, " +
            COLUMN_UPDATED + " TIMESTAMP DEFAULT CURRENT_TIMESTAMP, " +
            COLUMN_ISACTIVE + " INTEGER DEFAULT 1);";

    public static final String QUERY_CREATE_CONTACT = "CREATE TABLE IF NOT EXISTS " + TABLE_CONTACT + "(" +
            COLUMN_CONTACTID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
            COLUMN_CONTACTNAME + " TEXT NOT NULL, " +
            COLUMN_CONTACTPHONE + " TEXT NOT NULL, " +
            COLUMN_CONTACTREGID + " TEXT NOT NULL, " +
            COLUMN_CONTACTCREATED + " TIMESTAMP DEFAULT CURRENT_TIMESTAMP, " +
            COLUMN_CONTACTUPDATED + " TIMESTAMP DEFAULT CURRENT_TIMESTAMP, " +
            COLUMN_CONTACTISACTIVE + " INTEGER DEFAULT 1);";

    public static final String QUERY_CREATE_GROUP_CONTACT = "CREATE TABLE IF NOT EXISTS " + TABLE_CONTACTGROUP + "(" +
            COLUMN_CONTACTGROUPID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
            COLUMN_CONTACTGROUPCONTACTID + " INTEGER NOT NULL, " +
            COLUMN_CONTACTGROUPGROUPID + " INTEGER NOT NULL, " +
            COLUMN_CONTACTGROUPCREATED + " TIMESTAMP DEFAULT CURRENT_TIMESTAMP, " +
            COLUMN_CONTACTGROUPUPDATED + " TIMESTAMP DEFAULT CURRENT_TIMESTAMP, " +
            COLUMN_CONTACTGROUPISACTIVE + " INTEGER DEFAULT 1);";

    public static final String QUERY_CREATE_CONVERSATION = "CREATE TABLE IF NOT EXISTS " + TABLE_CONVERSATION + "(" +
            COLUMN_CONVERSATIONID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
            COLUMN_CONVERSATIONMESSAGE + " TEXT NOT NULL, " +
            COLUMN_CONVERSATIONOWNERID + " INTEGER NOT NULL, " +
            COLUMN_CONVERSATIONTOGROUPID + " INTEGER, " +
            COLUMN_CONVERSATIONTOCONTACTID + " INTEGER, " +
            COLUMN_CONVERSATIONDATETIME + " TIMESTAMP DEFAULT CURRENT_TIMESTAMP, " +
            COLUMN_CONVERSATIONISACTIVE + " INTEGER DEFAULT 1);";

    public DataHandler(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        try {
            db.execSQL(QUERY_CREATE_GROUP);
            db.execSQL(QUERY_CREATE_CONTACT);
            db.execSQL(QUERY_CREATE_GROUP_CONTACT);
            db.execSQL(QUERY_CREATE_CONVERSATION);
            Log.d("Info", "Created database tables successfully");
        }
        catch (Exception ex){
            Log.e("Error", ex.getMessage());
        }
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        Log.d("DATABASE", "Upgrading database from "+ oldVersion + " to " + newVersion);
    }
}
