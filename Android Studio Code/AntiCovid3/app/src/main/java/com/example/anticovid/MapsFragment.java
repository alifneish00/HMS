package com.example.anticovid;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class MapsFragment extends Fragment {

    private List<ListHospitals> listHospitals;
    String latitude;
    String longitude;

    private static final  String URL_HOSPITALS = "https://192.168.1.116:44322/api/hospitals";

    private OnMapReadyCallback callback = new OnMapReadyCallback() {

        /**
         * Manipulates the map once available.
         * This callback is triggered when the map is ready to be used.
         * This is where we can add markers or lines, add listeners or move the camera.
         * In this case, we just add a marker near Sydney, Australia.
         * If Google Play services is not installed on the device, the user will be prompted to
         * install it inside the SupportMapFragment. This method will only be triggered once the
         * user has installed Google Play services and returned to the app.
         */
        @Override
        public void onMapReady(GoogleMap googleMap) {
            LatLng leb = new LatLng(33, 35);
            googleMap.addMarker(new MarkerOptions().position(leb).title("Marker in lebanon"));
            googleMap.moveCamera(CameraUpdateFactory.newLatLng(leb));

            //code l data

            StringRequest stringRequest = new StringRequest(Request.Method.GET, URL_HOSPITALS,
                    new Response.Listener<String>() {
                        @Override
                        public void onResponse(String response) {
                            Log.d("my", response.toString());
                            try {
                                Log.d("my", "This is my message2");
                                //JSONObject jsonObject = new JSONObject(response);
                                //JSONArray array = jsonObject.getJSONArray("");
                                JSONArray array = new JSONArray(response);

                                for (int i = 0; i<array.length(); i++) {
                                    JSONObject o = array.getJSONObject(i);

                                    latitude = o.getString("Latitude");
                                    longitude = o.getString("Longitude");
                                    LatLng hospital = new LatLng(Double.parseDouble(latitude),
                                            Double.parseDouble(longitude));
                                    googleMap.addMarker(new MarkerOptions().position(hospital).title(o.getString("Name")));


                                }





                            } catch (JSONException e) {
                                e.printStackTrace();
                                Log.e("myD3", e.getMessage());
                            }

                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            Log.e("myError",error.getMessage());

                        }
                    });

            RequestQueue requestQueue = Volley.newRequestQueue(getContext());
            requestQueue.add(stringRequest);



        }
    };

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater,
                             @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        ViewGroup root= (ViewGroup) inflater.inflate(R.layout.fragment_maps, container, false);
        listHospitals = new ArrayList<>();
        return  root;
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        SupportMapFragment mapFragment =
                (SupportMapFragment) getChildFragmentManager().findFragmentById(R.id.map);
        if (mapFragment != null) {
            mapFragment.getMapAsync(callback);
        }
    }

    private void getData(){



    }
}