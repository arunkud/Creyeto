package com.heygo.arunkumar.common;

import android.content.Context;
import android.database.Cursor;
import android.net.Uri;
import android.telephony.TelephonyManager;
import android.view.View;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Arunkumar on 6/2/2015.
 */
public class Utility {
    public static ArrayList<String> GetCurrentPhoneNumbers(Context context){
        ArrayList<String> retValue = null;
        try {
            TelephonyManager tMgr = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);
            String mPhoneNumber = tMgr.getLine1Number();
            if(!mPhoneNumber.isEmpty()){
                retValue = new ArrayList<String>();
                retValue.add(mPhoneNumber);
            }
        }
        catch (Exception ex){
            Shared.logWarning("Unable to retrive phone number from TelephonyManager");
        }

        if(retValue == null){
            ArrayList list = GetPhoneNumberFromSMS(context);
            if(list != null){
                retValue = new ArrayList<String>();
                retValue.addAll(list);
            }
        }

        return retValue;
    }

    private static ArrayList GetPhoneNumberFromSMS(Context context){
        ArrayList<String> arrayList = new ArrayList<String>();
        try {
            Cursor cursor = context.getContentResolver().query(Uri.parse("content://sms/inbox"), null, null, null, null);
            if (cursor.moveToFirst()) {
                do {
                    String msgData = "";
                    for (int idx = 0; idx < cursor.getColumnCount(); idx++) {
                        int index = cursor.getColumnIndexOrThrow("creator");
                        arrayList.add(cursor.getString(index));
                    }
                    Shared.logInformation(msgData);
                    // use msgData
                } while (cursor.moveToNext());
            } else {
                // empty box, no SMS
            }

        }
        catch (Exception ex){

        }

        return arrayList;
    }
}
