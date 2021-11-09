import "dart:convert";

import "package:flutter/material.dart";
import "dart:collection";
import "package:http/http.dart" as http;

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: "Flutter Demo",
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: const LoginPage(title: "Login"),
    );
  }
}

class LoginPage extends StatefulWidget {

  final String title;

  const LoginPage({Key? key, required this.title}) : super(key: key);

  @override
  State<LoginPage> createState() => _LoginPageState();
  
}

class _LoginPageState extends State<LoginPage> {

  bool _passwordVisible = false;
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  HashMap form = HashMap<String, String>();

  void submit() async {
    _formKey.currentState?.validate();
    final response = await http.post(
      Uri.parse("EventualServerAddress"),
      headers: <String, String>{ "Content-Type": "application/json; charset=UTF-8"},
      body: jsonEncode(<String, String>{
        "email": form["email"],
        "password": form["password"]
      })
    );

    if (response.statusCode == 200) {
      print("Succesful login");
    } else {
      print("Unsuccesful login");
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.title),
      ),
      body: Form(
        key: _formKey,
        child: Padding(
          padding: EdgeInsets.all(MediaQuery.of(context).size.width * 0.2),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: <Widget>[
              TextFormField(
                decoration: const InputDecoration(
                  hintText: "e-mail",
                ),
                validator: (String? value) {
                  form["email"] = value;
                  return null;
                },
              ),
              TextFormField(
                obscureText: !_passwordVisible,
                decoration: InputDecoration(
                  hintText: "password",
                  suffixIcon: IconButton(
                    icon: Icon(
                      _passwordVisible ? Icons.visibility : Icons.visibility_off,
                      color: Theme.of(context).primaryColorDark,
                      ),
                    onPressed: () {
                      setState(() {
                          _passwordVisible = !_passwordVisible;
                      });
                    },
                  ),
                ),
                validator: (String? value) {
                  form["password"] = value;
                  return null;
                },
              ),
              Padding(
                padding: const EdgeInsets.symmetric(vertical: 16.0),
                child: ElevatedButton(
                  child: const Text("Submit"),
                  onPressed: submit,
                  ),
              ),
            ],
          ),
        )
      ),
    );
  }
}