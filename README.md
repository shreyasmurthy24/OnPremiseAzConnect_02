Here I'm trying to insert data into Az SQL (ASQL) using an on-premise wpf application as a source through an Azure Function App. 
You need 2 instances of VS, one for WPF and other for Function App.

WPF --> Az Function App --> ASQL

This is a 2 repository application.

1. OnPremiseAzConnect
2. OnPremiseAzConnect_2
--------------------------------------------------------
1. WPF application:

Is a simple WFP applicaiton, which reads the user inputs and connects to the Az Function App. Line # 61, of this projects 
MainWindow.xaml.cs has a URI. This is a uri of the Az function app.

Create a function on the first basis, and capture the function app url, by running the app from the storage emulator. Pass the url 
in line # 61. This establishes a connectivity between wpf and function app.

Degugging from line # 65, takes the control to another instance of VS where the function app code is written.

2. Azure Function App:

I added a HttpTrigger, with .Net core framework. This trigger's, Run(..) method captures the information from wpf app, from the 
parameter HttpRequest req. This also inserts the data into ASQL. Data movement between on-premise and function app happens in json 
format. We serialize and de-serialize the data. This is a POST Http method.

Deploy the function app in the last.

Note: Run the Function app first and later run the wpf application. The wpf application is dependent on Az function app.

Note: We can use .Net framework as well for HttpTrigger.
