package com.example.anticovid;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

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
 * Use the {@link SecondFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class SecondFragment extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;
    private List<ListReservationRequests> listReservationRequests;
    private static final  String URL_Requests = "https://192.168.1.116:44322/api/ReservationRequests";

    private RecyclerView recyclerView;
    private RecyclerView.Adapter adapter;

    public SecondFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment SecondFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static SecondFragment newInstance(String param1, String param2) {
        SecondFragment fragment = new SecondFragment();
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
        ViewGroup root = (ViewGroup)inflater.inflate(R.layout.fragment_second, container, false);

        recyclerView = root.findViewById(R.id.recyclerRequests);
        recyclerView.setHasFixedSize(true);
        recyclerView.setLayoutManager(new LinearLayoutManager(root.getContext()));
        listReservationRequests = new ArrayList<>();
        recyclerView.setAdapter(new RequestsRecycleAdapter(listReservationRequests,getContext()));

        loadRecyclerViewData();

        return root;
    }

    private void loadRecyclerViewData() {
        trustEveryone();

        StringRequest stringRequest = new StringRequest(Request.Method.GET, URL_Requests,
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
                                ListReservationRequests request = new ListReservationRequests (
                                        o.getString("Id"),
                                        o.getString("Status"),
                                        o.getString("RequestDate"),
                                        o.getString("UserId"),
                                        o.getString("HospitalId"),
                                        o.getString("ReservationId"),
                                        o.getString("Discription")
                                );
                                SharedPreferences sp = getActivity().getSharedPreferences("MyUserPrefs", Context.MODE_PRIVATE);
                                if(o.getString("UserId").equals(sp.getString("uName",""))){
                                    listReservationRequests.add(request);
                                }



                                    //Toast.makeText(getActivity(),"You Dont Have Any Requests",Toast.LENGTH_LONG).show();

                            }
                           adapter = new RequestsRecycleAdapter(listReservationRequests,getContext());
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