package com.heygo.arunkumar.common;

import com.example.arunkumar.test10.R;
import com.heygo.arunkumar.common.Shared;
import com.heygo.arunkumar.data.Contact;

import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URI;
import java.net.URL;
import java.net.URLEncoder;
import java.util.ArrayList;
import java.util.List;

public class ServerContactHandler {
    /**
     * This constructs the URL that allows you to manage your database,
     * collections and documents
     *
     * @return
     */
    private static String getBaseUrl() {
        return "http://192.168.1.5:8090";
    }

    private static Contact[] ParseContactsResponse(String responseData) {
        Contact[] contacts = null;
        try {
            JSONArray jsonResponse = new JSONArray(responseData);
            int lengthJsonArr = jsonResponse.length();
            contacts = new Contact[lengthJsonArr];
            for (int i = 0; i < lengthJsonArr; i++) {
                JSONObject jsonChildNode = jsonResponse.getJSONObject(i);
                Contact contact = new Contact();
                contact.ID = Integer.parseInt(jsonChildNode.optString("contact_id"));
                contact.Name = jsonChildNode.optString("full_name");
                contact.PhoneNumber = jsonChildNode.optString("phone_number");
                contact.RegisterKey = jsonChildNode.optString("key");
                contact.IsActive = true;
                contacts[i] = contact;
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return contacts;
    }

    private static Contact ParseContactFromResponse(String responseData) {
        try {
            JSONObject jsonResponse = new JSONObject(responseData);
            Contact contact = new Contact();
            contact.Name = jsonResponse.optString("full_name").toString();
            contact.PhoneNumber = jsonResponse.optString("phone_number").toString();
            contact.RegisterKey = jsonResponse.optString("key").toString();
            contact.IsActive = Boolean.parseBoolean(jsonResponse.optString("is_active").toString());
            return contact;
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return null;
    }

    private static String RetrieveFromServer(String query) {
        try {
            HttpClient client = new DefaultHttpClient();
            HttpGet request = new HttpGet(query);
            HttpResponse response = client.execute(request);
            int statusCode = response.getStatusLine().getStatusCode();
            if (statusCode >= 200) {
                BufferedReader rd = new BufferedReader(
                        new InputStreamReader(response.getEntity().getContent()));

                StringBuffer result = new StringBuffer();
                String line = "";
                while ((line = rd.readLine()) != null) {
                    result.append(line);
                }
                return result.toString();
            }
        } catch (Exception ex) {
            Shared.logError("Error in retrieving data from server database!");
        }
        return null;
    }

    public static String RetrieveFromServerPOST(String query, List<NameValuePair> nameValuePairs){
        HttpClient httpClient = new DefaultHttpClient();
        HttpPost request = new HttpPost(query);
        try {
            request.addHeader("content-type", "text/html");
            request.setEntity(new UrlEncodedFormEntity(nameValuePairs));
            HttpResponse response = httpClient.execute(request);
            int statusCode = response.getStatusLine().getStatusCode();
            if (statusCode >= 200)
                return EntityUtils.toString(response.getEntity());
            else
                return null;
        }
        catch (Exception ex){
            Shared.logError(ex.getMessage());
            return null;
        }
    }

    public static Contact getContact(String phone) {
        String query = null;
        try {
            query = getBaseUrl() + "/study/gcmcontact.php";
        }
        catch (Exception ex){
            Shared.logError(ex.getMessage());
        }

        List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>(1);
        nameValuePairs.add(new BasicNameValuePair("ph", phone));

        String responseData = RetrieveFromServerPOST(query, nameValuePairs);
        if (responseData != null && !responseData.isEmpty()) {
            return ParseContactFromResponse(responseData);
        }
        return null;
    }

    public static Boolean SendMessage(String from_phone, String registerationId, String conv_text){
        HttpClient httpClient = new DefaultHttpClient();
        HttpPost request = new HttpPost(getBaseUrl() + "/study/gcmsend.php");
        try {
            request.addHeader("content-type", "text/html");
            List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>(3);
            nameValuePairs.add(new BasicNameValuePair("ph", from_phone));
            nameValuePairs.add(new BasicNameValuePair("rk", registerationId));
            nameValuePairs.add(new BasicNameValuePair("msg", conv_text));
            request.setEntity(new UrlEncodedFormEntity(nameValuePairs));
            HttpResponse response = httpClient.execute(request);
            int statusCode = response.getStatusLine().getStatusCode();
            String responseBody = EntityUtils.toString(response.getEntity());
            if (statusCode >= 200)
                return true;
            else
                return false;
        }
        catch (Exception ex){
            Shared.logError(ex.getMessage());
            return false;
        }
    }

    public static Boolean saveContact(String contactName, String phoneNumber, String regKey){
        HttpClient httpClient = new DefaultHttpClient();
        HttpPost request = new HttpPost(getBaseUrl() + "/study/gcmreceiver.php");
        try {
            request.addHeader("content-type", "text/html");
            List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>(3);
            nameValuePairs.add(new BasicNameValuePair("ph", phoneNumber));
            nameValuePairs.add(new BasicNameValuePair("cn", contactName));
            nameValuePairs.add(new BasicNameValuePair("rk", regKey));
            request.setEntity(new UrlEncodedFormEntity(nameValuePairs));
            HttpResponse response = httpClient.execute(request);
            int statusCode = response.getStatusLine().getStatusCode();
            String responseBody = EntityUtils.toString(response.getEntity());
            if (statusCode >= 200)
                return true;
            else
                return false;
        }
        catch (Exception ex){
            Shared.logError(ex.getMessage());
            return false;
        }
    }
}
