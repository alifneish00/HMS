package com.example.anticovid;

import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

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

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.security.SecureRandom;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;
import java.util.ArrayList;
import java.util.List;

import javax.net.ssl.HostnameVerifier;
import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSession;
import javax.net.ssl.X509TrustManager;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link FirstFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class FirstFragment extends Fragment {

    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    // TODO: Rename parameter arguments, choose names that match

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;
    private List<ListHospitals> listHospitals;
    private static final  String URL_HOSPITALS = "https://192.168.1.116:44322/api/hospitals";

    private RecyclerView recyclerView;
    private RecyclerView.Adapter adapter;

    public FirstFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment FirstFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static FirstFragment newInstance(String param1, String param2) {
        FirstFragment fragment = new FirstFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }


    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        ViewGroup root = (ViewGroup) inflater.inflate(R.layout.fragment_first, container, false);

        recyclerView = root.findViewById(R.id.recyclerView);
        recyclerView.setHasFixedSize(true);
        recyclerView.setLayoutManager(new LinearLayoutManager(root.getContext()));
        listHospitals = new ArrayList<>();
        recyclerView.setAdapter(new MyRecycleAdapter(listHospitals,getContext()));
        loadRecyclerViewData();






                return root;
    }

   // @Override
   // public void onStop() {
       // super.onStop();
        //getActivity().finish();
   // }

    private void loadRecyclerViewData() {
        trustEveryone();

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
                                ListHospitals hospital = new ListHospitals(
                                        o.getString("Name"),
                                        o.getString("Address"),
                                        o.getString("Phone"),
                                        o.getString("Latitude"),
                                        o.getString("Longitude")
                                );
                                listHospitals.add(hospital);
                            }
                            adapter = new MyRecycleAdapter(listHospitals,getContext());
                            recyclerView.setAdapter(adapter);

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

    private void trustEveryone() {
        try {
            HttpsURLConnection.setDefaultHostnameVerifier(new HostnameVerifier(){
                public boolean verify(String hostname, SSLSession session) {
                    return true;
                }});
            SSLContext context = SSLContext.getInstance("TLS");
            context.init(null, new X509TrustManager[]{new X509TrustManager(){
                public void checkClientTrusted(X509Certificate[] chain,
                                               String authType) throws CertificateException {}
                public void checkServerTrusted(X509Certificate[] chain,
                                               String authType) throws CertificateException {}
                public X509Certificate[] getAcceptedIssuers() {
                    return new X509Certificate[0];
                }}}, new SecureRandom());
            HttpsURLConnection.setDefaultSSLSocketFactory(
                    context.getSocketFactory());
        } catch (Exception e) { // should never happen
            e.printStackTrace();
        }
    }
}