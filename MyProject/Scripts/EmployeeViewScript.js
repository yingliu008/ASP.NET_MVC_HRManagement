var HomeView = function () {
    var isCollapsed = true;
};
HomeView.ViewModel = null;

function employeeModel(viewModel) {
    var self = this;
    self.employee = ko.observable(viewModel);

    self.deleteUser = function () {
        var koData = ko.toJS(self.employee());
        var data = { JsonData: JSON.stringify(koData) };
        $.ajax({
            cache: false,
            url: "Home/RemoveEmployee/",
            type: 'POST',
            data: data,
            success: function (result) {
                var data = JSON.parse(result);

                if (data) {
                    HomeView.ViewModel.clearTable();
                    HomeView.ViewModel.getEmployeeList();
                }

            },
            error: function (result) {
                alert('not deleted!');
            }
        });
    };

    self.EditUser = function () {
        var someone = self.employee();
        HomeView.ViewModel.Id(someone.Id);
        HomeView.ViewModel.firstName(someone.FirstName);
        HomeView.ViewModel.lastName(someone.LastName);
        HomeView.ViewModel.address(someone.Address);
        HomeView.ViewModel.city(someone.City);
        HomeView.ViewModel.state(someone.State);
        HomeView.ViewModel.phoneNumber(someone.PhoneNumber);
        HomeView.ViewModel.email(someone.Email);       
    };
}


function HomeViewModel(){
    var self = this;
    
    self.Id = ko.observable(0);
    self.firstName = ko.observable('');
    self.lastName = ko.observable('');
    self.address = ko.observable('');
    self.city = ko.observable('');
    self.state = ko.observable('');
    self.phoneNumber = ko.observable('');
    self.email = ko.observable('');
    self.fullName = ko.computed(function () { return self.firstName() + ' ' + self.lastName(); });
    self.employeeList = ko.observableArray();
    self.warningBool = ko.observable(0);
    self.hasSavedText = ko.observable('');
    self.hasSavedColor = ko.observable(true);

    self.saveEmployee = function () {
        var koData = ko.toJS(self);
        var originalId = koData.Id;
        var data = { JsonData : JSON.stringify(koData) };
        $.ajax({
            cache: false,
            url: "Home/SaveData/",
            type: 'POST',
            data: data,
            success: function (result) {
                var data = JSON.parse(result);
                var alertText;

                if (originalId <=0)
                {
                   alertText = self.fullName() + ' has been saved!';
                }
                else {
                    alertText = self.fullName() + ' has been updated!';
                }

                $("#saveEmployeeAlert").show();
                self.clearTable();
                self.getEmployeeList();
                self.clearForm();
                self.hasSavedText(alertText);
                self.warningBool(0);
                self.hasSavedColor(true);              
            },
            error: function (result) {
                alert(self.fullName() + ' has not been saved!');
                self.hasSavedText("An error has occured while saving your request. Could not saved!")
                self.hasSavedColor(false);
                self.warningBool(1);
                $("#saveEmployeeAlert").show();
            }
        });
    };

    self.getEmployeeList = function(){     
        var data = { };
        $.ajax({
            cache: false,
            url: "Home/GetEmployeeList/",
            type: 'POST',
            data: data,
            success: function (result) {
                var data = JSON.parse(result);
                for (var i = 0; i < data.length; i++) {
                    self.employeeList.push(new employeeModel(data[i]));
                }
            },
            error: function (result) {
            }
        });
    }

    self.clearTable = function () {
        self.employeeList.destroyAll();
    };

    self.clearForm = function () {
        self.firstName('');
        self.lastName('');
        self.address('');
        self.city('');
        self.state('');
        self.phoneNumber('');
        self.email('');
    }
};


//HomeView.MakeGrid = function () {

//    var url = "Home/SearchList/";

//    var volunteerGrid = jQuery("#MainGrid").jqGrid({

//        url: url,
//        datatype: "json",
//        colNames: ['Id', 'FirstName', 'LastName', 'Address', 'City', 'State', 'PhoneNumber', 'Email'],
//        colModel: [
//            { name: 'Id', index: 'Id', width: 55, hidden: true },
//            { name: 'FirstName', index: 'FirstName', width: 100 },
//            { name: 'LastName', index: 'LastName', width: 100 },
//            { name: 'Address', index: 'Address', width: 100 },
//            { name: 'City', index: 'City', width: 55, hidden: true },
//            { name: 'State', index: 'State', width: 100 },
//            { name: 'PhoneNumber', index: 'PhoneNumber', width: 100 },
//            { name: 'Email', index: 'Email', width: 100 },
//        ],
//        rowNum: 200,
//        rowList: [100, 200, 500],
//        pager: '#pager2',
//        width: 800,
//        height: 300,
//        sortname: 'FirstName',
//        sortorder: "asc",
//        viewrecords: false,
//        hidegrid: false,
//        caption: "Employee List",
//        mtype:'GET',
//        loadComplete: function (data) {
//            $("#sp_1_pager2").text("*");
//        }
//    });
//    jQuery("#list2").jqGrid('navGrid', '#pager2', { edit: false, add: false, del: false });
//    $("#btnSearch").click(function () {
//        volunteerGrid.trigger("reloadGrid");
//    });
//}


$(document).ready(function () {
    HomeView.ViewModel = new HomeViewModel();

    ko.applyBindings(HomeView.ViewModel, $('#dvInputForm').get(0));
    HomeView.ViewModel.getEmployeeList();

});