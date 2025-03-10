angular.module('umbraco').config(function (dashboardServiceProvider) {
    dashboardServiceProvider.section('content').add({
        alias: 'registrationDashboard',
        name: 'Registration Dashboard',
        view: '/App_Plugins/RegistrationDashboard/dashboard.html',
        weight: 10
    });
});
