package com.heygo.arunkumar.activity.fragment;

import android.support.v4.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.arunkumar.test10.R;

/**
 * Created by Arunkumar on 5/16/2015.
 */
public class FeedsFragment extends Fragment {
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        return inflater.inflate(R.layout.feeds_main, container, false);

    }
}
