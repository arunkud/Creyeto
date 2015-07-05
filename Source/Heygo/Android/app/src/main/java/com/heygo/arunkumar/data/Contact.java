package com.heygo.arunkumar.data;

import java.io.Serializable;
import java.util.Date;
import java.util.List;

/**
 * Created by Arunkumar on 5/22/2015.
 */
public class Contact implements Serializable {
    public int ID;
    public String Name;
    public String PhoneNumber;
    public String RegisterKey;
    public Date Created;
    public Date Updated;
    public Boolean IsActive;
}
