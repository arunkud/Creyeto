package com.heygo.arunkumar.activity.ViewHandles;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.app.FragmentManager;

import com.heygo.arunkumar.activity.fragment.FeedsFragment;
import com.heygo.arunkumar.activity.fragment.GroupsFragment;
import com.heygo.arunkumar.activity.fragment.PrivateChatFragment;

/**
 * Created by Arunkumar on 5/16/2015.
 */
public class ViewPageSelector extends FragmentPagerAdapter {
    public ViewPageSelector(FragmentManager fm) {
        super(fm);
    }

    @Override
    public Fragment getItem(int i) {
        switch (i){
            case 0:
                return new PrivateChatFragment();
            case 1:
                return new GroupsFragment();
            case 2:
                return new FeedsFragment();
        }
        return null;
    }

    @Override
    public int getCount() {
        return 3;
    }
}
