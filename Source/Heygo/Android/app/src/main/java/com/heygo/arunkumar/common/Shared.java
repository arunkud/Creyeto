package com.heygo.arunkumar.common;

import android.content.Context;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.util.Log;


/**
 * Created by Arunkumar on 5/29/2015.
 */
public class Shared {
    private static String logError = "Heygo Error";
    private static String logWarning = "Heygo Warning";
    private static String logInformation = "Heygo Information";
    private static String logDebug = "Heygo Debug";

    private static String CONST_RESOURCE_FILENAME = "HeyGo_Preferences";
    public static final String CONST_GOOGLE_PROPERTY_REG_ID = "gcm_registration_id";
    public static final String CONST_APP_VERSION_ID = "app_version_id";
    public static final String CONST_PHONE_NUMBER = "phone_number";
    public static String CONST_GOOGLE_PROJECT_ID = "208148448217";


    public static void logError(String errorMessage){
        Log.e(logError, errorMessage);
    }

    public static void logDebug(String message){
        Log.d(logDebug, message);
    }

    public static void logWarning(String message){
        Log.w(logWarning, message);
    }

    public static void logInformation(String message){
        Log.i(logInformation, message);
    }



    public static String getConfiguration(Context context, String name) {
        SharedPreferences preferences = context.getSharedPreferences(CONST_RESOURCE_FILENAME, Context.MODE_PRIVATE);
        if(preferences == null) {
            logError("Unable to fetch the preferences from " + CONST_RESOURCE_FILENAME);
            return null;
        }
        return preferences.getString(name, null);
    }

    public static void setConfiguration(Context context, String name, String value) {
        SharedPreferences preferences = context.getSharedPreferences(CONST_RESOURCE_FILENAME, Context.MODE_PRIVATE);
        if(preferences == null)
            logError("Unable to fetch the preferences from " + CONST_RESOURCE_FILENAME);

        SharedPreferences.Editor editor = preferences.edit();
        editor.putString(name, value);
        editor.commit();
    }

    public static String getAppVersion(Context context) {
        try {
            PackageInfo packageInfo = context.getPackageManager().getPackageInfo(context.getPackageName(), 0);
            return String.valueOf(packageInfo.versionCode);
        } catch (PackageManager.NameNotFoundException e) {
            // should never happen
            throw new RuntimeException("Could not get package name: " + e);
        }
    }
}
