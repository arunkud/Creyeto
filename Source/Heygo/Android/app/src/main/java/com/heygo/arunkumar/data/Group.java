package com.heygo.arunkumar.data;

import java.util.Date;
import java.util.List;

/**
 * Created by Arunkumar on 5/22/2015.
 */
public class Group {
    public int ID;
    public String Name;
    public Date Created;
    public Date Updated;
    public Boolean IsActive;
    public List<Contact> Contacts;
}
