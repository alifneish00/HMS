package com.example.anticovid;

import android.app.Dialog;
import android.content.Context;
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

import java.util.List;

public class RequestsRecycleAdapter extends RecyclerView.Adapter<RequestsRecycleAdapter.MyViewHolder> {

    private List<ListReservationRequests> listReservationRequests;
    private Context context;


    public RequestsRecycleAdapter(List<ListReservationRequests> listReservationRequests, Context context) {
        this.listReservationRequests = listReservationRequests;
        this.context = context;
    };


    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.card_view_request, parent, false);



        return  new MyViewHolder(v);
    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
        ListReservationRequests listReservationRequests1 = listReservationRequests.get(position);

        holder.resHospitalName.setText("Hospital: "+listReservationRequests1.getHosId());
        holder.reqDate.setText("Date Requested: "+listReservationRequests1.getReqDate());
        holder.reqDisc.setText("Request Status: "+listReservationRequests1.getDesc());


    }

    @Override
    public int getItemCount() {
        return listReservationRequests.size();
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {

        private TextView resHospitalName, reqDate, reqDisc;

        public MyViewHolder(@NonNull View itemView) {
            super(itemView);

            resHospitalName = itemView.findViewById(R.id.reservedHospitalName);
            reqDate = itemView.findViewById(R.id.requestDate);
            reqDisc = itemView.findViewById(R.id.discReq);

        }
    }

}
