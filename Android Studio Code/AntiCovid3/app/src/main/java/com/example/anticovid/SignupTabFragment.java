package com.example.anticovid;

import android.content.Intent;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONException;
import org.json.JSONObject;

import java.security.SecureRandom;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;

import javax.net.ssl.HostnameVerifier;
import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSession;
import javax.net.ssl.X509TrustManager;

public class SignupTabFragment extends Fragment {

    EditText email,phone,pass1,pass2,address,dob;
    Button signup;
    float v=0;

    private static String URL_Signup = "https://192.168.1.116:44322/api/CreateUser?";

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        ViewGroup root = (ViewGroup) inflater.inflate(R.layout.signup_tab_fragment,container,false);

        email = root.findViewById(R.id.email2);
        phone = root.findViewById(R.id.phone);
        pass1 = root.findViewById(R.id.pass1);
        pass2 = root.findViewById(R.id.pass2);
        address = root.findViewById(R.id.regAddress);
        dob = root.findViewById(R.id.regDate);
        signup = root.findViewById(R.id.button2);

        email.setTranslationX(800);
        phone.setTranslationX(800);
        pass1.setTranslationX(800);
        pass2.setTranslationX(800);
        address.setTranslationX(800);
        dob.setTranslationX(800);
        signup.setTranslationX(800);


        email.setAlpha(v);
        phone.setAlpha(v);
        pass1.setAlpha(v);
        pass2.setAlpha(v);
        address.setAlpha(v);
        dob.setAlpha(v);
        signup.setAlpha(v);

        email.animate().translationX(0).alpha(1).setDuration(800).setStartDelay(300).start();
        phone.animate().translationX(0).alpha(1).setDuration(800).setStartDelay(300).start();
        pass1.animate().translationX(0).alpha(1).setDuration(800).setStartDelay(300).start();
        pass2.animate().translationX(0).alpha(1).setDuration(800).setStartDelay(300).start();
        address.animate().translationX(0).alpha(1).setDuration(800).setStartDelay(300).start();
        dob.animate().translationX(0).alpha(1).setDuration(800).setStartDelay(300).start();
        signup.animate().translationX(0).alpha(1).setDuration(800).setStartDelay(300).start();

        signup.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                //getting input from user
                try {
                    String emailU, passU1, passU2, phoneU, addressU, dobU;
                    emailU = email.getText().toString();
                    passU1 = pass1.getText().toString();
                    passU2 = pass2.getText().toString();
                    phoneU = phone.getText().toString();
                    addressU = address.getText().toString();
                    dobU = dob.getText().toString();

                    //Checking Empty Input
                    if (TextUtils.isEmpty(emailU))
                    {
                        email.setError("Please Enter Your Email");
                        email.requestFocus();
                        //return;
                    }

                    if (TextUtils.isEmpty(passU1))
                    {
                        pass1.setError("Please Enter a Password");
                        pass1.requestFocus();
                        //return;
                    }

                    if (TextUtils.isEmpty(passU2))
                    {
                        pass2.setError("Please Confirm the Password");
                        pass2.requestFocus();
                        //return;
                    }

                    if (TextUtils.isEmpty(phoneU))
                    {
                        phone.setError("Please Enter a Phone Number");
                        phone.requestFocus();
                        //return;
                    }

                    if (TextUtils.isEmpty(addressU))
                    {
                        address.setError("Please Enter Your Address");
                        address.requestFocus();
                        //return;
                    }

                    if (!passU1.equals(passU2)){
                        Toast.makeText(getActivity(),"Password doesn't match",Toast.LENGTH_LONG).show();
                        //return;
                    }
                    trustEveryone();

                    URL_Signup ="https://192.168.1.116:44322/api/CreateUser?username="+emailU+"&password="+passU1+"&address="+addressU+"&mobile="+phoneU+"&dob="+dobU;

                    StringRequest stringRequest = new StringRequest(Request.Method.GET, URL_Signup,
                            new Response.Listener<String>() {
                                @Override
                                public void onResponse(String response) {

                                    //JSONObject obj = new JSONObject(response);
                                    Log.d("my", response);

                                }
                            },
                            new Response.ErrorListener() {
                                @Override
                                public void onErrorResponse(VolleyError error) {
                                    Toast.makeText(getContext(), error.getMessage(), Toast.LENGTH_SHORT).show();
                                }
                            }
                    );
                    RequestQueue requestQueue = Volley.newRequestQueue(getActivity());
                    requestQueue.add(stringRequest);





                }
                catch (Exception e) {
                    e.printStackTrace();
                }





            }
        });




        return root;


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
