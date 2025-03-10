(function () {
    'use strict';

    angular.module('umbraco').controller('RegistrationDashboardController', function ($http, notificationsService) {
        var vm = this;

        vm.loading = true;
        vm.registrationData = [];

        // Fetch registration data
        $http.get('/umbraco/api/RegistrationApi/GetAll').then(function (response) {
            vm.registrationData = response.data;
            vm.loading = false;
        }, function (error) {
            notificationsService.error("Error", "Failed to load registration data.");
            vm.loading = false;
        });
    });
})();
