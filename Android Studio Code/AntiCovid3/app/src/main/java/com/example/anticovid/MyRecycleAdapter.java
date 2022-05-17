package com.example.anticovid;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;

import java.security.SecureRandom;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;
import java.util.List;

import javax.net.ssl.HostnameVerifier;
import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSession;
import javax.net.ssl.X509TrustManager;

public class MyRecycleAdapter extends RecyclerView.Adapter<MyRecycleAdapter.MyViewHolder> {

    private List<ListHospitals> listHospitals;
    private Context context;
    private Dialog myDialog;

    public MyRecycleAdapter(List<ListHospitals> listHospitals, Context context) {

        this.listHospitals = listHospitals;
        this.context = context;

    };

    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.card_view_item, parent, false);



        return  new MyViewHolder(v);
    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
        ListHospitals listHospital = listHospitals.get(position);

        holder.hospitalNameCard.setText("Hospital : " + listHospital.getName());
        holder.hospitalAddressCard.setText("Address : " + listHospital.getAddress());
        holder.hospitalPhoneCard.setText("Phone : " + listHospital.getPhone());

        // Dialog ini
        myDialog = new Dialog(context);
        myDialog.setContentView(R.layout.pop_up_window);
        SharedPreferences sp = context.getSharedPreferences("MyUserPrefs", Context.MODE_PRIVATE);



        holder.reservationBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                TextView dialog_name = (TextView)  myDialog.findViewById(R.id.reservationTitle);
                dialog_name.setText("Reservation at " + listHospital.getName() + " Hospital");
                Button dialog_button = (Button) myDialog.findViewById(R.id.requestReservation);
                dialog_button.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        String uName = sp.getString("uName","");
                        String hosId = listHospital.getName();
                        trustEveryone();
                        String URL_AddReservationRequest = "https://192.168.1.116:44322/api/AddReservationRequest?username="+uName+"&HospitalId="+hosId;

                        StringRequest stringRequest = new StringRequest(Request.Method.GET, URL_AddReservationRequest,
                                new Response.Listener<String>() {
                                    @Override
                                    public void onResponse(String response) {
                                        Toast.makeText(context, response, Toast.LENGTH_LONG).show();
                                    }
                                }, new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                Toast.makeText(context,error.toString(),Toast.LENGTH_LONG).show();

                            }
                        }
                        );
                        RequestQueue requestQueue = Volley.newRequestQueue(context);
                        requestQueue.add(stringRequest);



                    }

                });



                //Toast.makeText(context,"Test"+String.valueOf(holder.getAdapterPosition()),Toast.LENGTH_LONG).show();

                myDialog.show();





            }
        });

    }

    @Override
    public int getItemCount() {
        return listHospitals.size();
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        private TextView hospitalNameCard, hospitalAddressCard, hospitalPhoneCard;
        private Button reservationBtn;
        public MyViewHolder(@NonNull View itemView) {
            super(itemView);
            hospitalNameCard = itemView.findViewById(R.id.hospitalNameCard);
            hospitalAddressCard = itemView.findViewById(R.id.hospitalAddressCard);
            hospitalPhoneCard = itemView.findViewById(R.id.hospitalPhoneCard);
            reservationBtn = itemView.findViewById(R.id.reservationBtn);

        }
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
