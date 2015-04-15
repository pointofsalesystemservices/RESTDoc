/// <reference path="_references.js"/>
/// <reference path="~/Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="~/Scripts/knockout-3.3.0.debug.js" />
/// <reference path="~/Scripts/knockout.mapping-latest.debug.js" />
//api/restapi/4463d375-1ee2-e411-82b3-00259027b2bc/pathgroup/4563d375-1ee2-e411-82b3-00259027b2bc/path/4663d375-1ee2-e411-82b3-00259027b2bc/verb


var UrlBuildingBlock = function (component, id) {
    var self = this;
    if (component === null || component === undefined) {
        throw new Error("Component must be specified");
    }

    if (id === undefined) {
        id = null;
    }

    self.component = component;
    self.id = id;
    self.parent = null;
    self.prefix = null;
    var trimEnd = function (character, string) {
        character = character ? character : " ";
        var i = string.length - 1;
        for (; i >= 0 && string.charAt(i) == character; i--);
        return string.substring(0, i + 1);
    }

    var trimStart = function (character, string) {
        var startIndex = 0;

        while (string[startIndex] === character) {
            startIndex++;
        }

        return string.substr(startIndex);
    }

    self.toUrl = function () {
        var thisobj = trimEnd("/", self.component) + "/";
        if (self.id) {
            thisobj += self.id;
        }

        if (self.parent) {
            thisobj = trimEnd("/", self.parent.toUrl()) + "/" + trimStart("/", thisobj);
        }

        if (self.prefix) {
            thisobj = trimEnd("/", self.prefix) + "/" + trimStart("/", thisobj);
        }

        return thisobj;
    }



    self.createChild = function (component, id) {
        return self.makeChild(new UrlBuildingBlock(component, id));
    }

    self.makeChild = function (urlBuildingBlock) {
        urlBuildingBlock.parent = self;
        return urlBuildingBlock;
    }
}

var rootBuildingBlock = new UrlBuildingBlock("api");

var RestApiViewModel = function () {
    var self = this;
    self.isViewModel = ko.observable(true);
    self.id = ko.observable();
    self.name = ko.observable();
    self.description = ko.observable();
    self.creator = ko.observable();
    self.version = ko.observable();
    self.created = ko.observable().extend({ moment: {} });
    self.lastModified = ko.observable().extend({ moment: {} });
    self.loaded = ko.observable(true);
    self.pathGroups = ko.observableArray([]);

    self.selectedPathGroup = ko.observable(null);
    self.editMode = ko.observable(false);


    self.buildingBlock = function () {
        return rootBuildingBlock.createChild("restapi", self.id());
    }

    self.url = ko.computed(function () {
        return self.buildingBlock().toUrl();
    });

    self.selectPathGroup = function (object) {
        self.selectedPathGroup(object);
    }

    self.edit = function (object) {
        self.selectPathGroup(object);
        if (object && !self.editMode()) {
            self.editMode(true);
        }
    }

    self.cancelEdit = function () {
        self.editMode(false);
        if (self.selectedPathGroup()) {
            self.selectedPathGroup().refresh();
        }
    }

    var getPathGroupUrl = function (pathGroup) {
        var ret = self.buildingBlock().createChild("pathgroup");
        if (pathGroup !== undefined && pathGroup !== null)
            ret.id = pathGroup.id;
        return ret.toUrl();
    }

    self.refresh = function () {
        var url = self.buildingBlock().toUrl();

        return $.ajax({
            dataType: "json",
            url: url,
            accepts: "application/json"
        }).done(function (data) {
            ko.mapping.fromJS(data, {}, self);
        });
    }

    self.save = function () {
        var url = self.buildingBlock().toUrl();
        var method = self.id() === null ? "POST" : "PUT";
        return $.ajax({
            url: url,
            method: method,
            dataType: "json",
            accepts: "application/json",
            contentType: "application/json",
            data: ko.mapping.toJSON(self)
        }).done(function (data) {
            ko.mapping.fromJS(data, {}, self);
        });
    }

    var getPathGroupViewModels = function (pathGroup) {
        return $.ajax({
            dataType: "json",
            url: getPathGroupUrl(pathGroup),
            accepts: "application/json"
        }).done(function (data) {
            if (pathGroup !== undefined && pathGroup !== null) {
                var item = ko.utils.arrayFirst(self.pathGroups, function (item) {
                    return item.id() === data.id;
                });

                if (!item || !item.isViewModel || !item.isViewModel()) {
                    if (item) {
                        self.pathGroups.remove(item);
                    }
                    item = new PathGroupViewModel(self);
                    ko.mapping.fromJS(data, {}, item);

                    self.pathGroups.push(item);
                } else {
                    ko.mapping.fromJS(data, {}, item);
                }
            } else {
                var newViewModels = data.map(function (item) {
                    var newViewModel = new PathGroupViewModel(self);
                    ko.mapping.fromJS(item, {}, newViewModel);
                    return newViewModel;
                });

                self.pathGroups.removeAll();
                $.each(newViewModels, function (index, viewModel) {
                    self.pathGroups.push(viewModel);
                });
            }
        }).fail(function (xhr) {
            if (xhr.statusCode === 404) {
                // Record not found.
            } else {
                // Actual error...
            }
        });
    }

    self.loadPathGroup = function (item) {
        return getPathGroupViewModels(item);
    }

    self.isLoaded = function (item) {
        if (!item.loaded)
            return false;
        return true;
    }

}

var PathGroupViewModel = function (parentViewModel) {
    var self = this;
    self.isViewModel = ko.observable(true);
    self.id = ko.observable();
    self.basePath = ko.observable();
    self.description = ko.observable();
    self.apiPaths = ko.observableArray([]);
    self.loaded = ko.observable(true);
    var parent = parentViewModel;

    self.refresh = function () {

    }


    self.buildingBlock = function () {
        return parent.buildingBlock.createChild("pathgroup", self.id());
    }
}


var PathViewModel = function (parentViewModel) {
    var self = this;
    self.isViewModel = ko.observable(true);
    self.id = ko.observable();
    self.path = ko.observable();
    self.description = ko.observable();
    self.apiVerbs = ko.observableArray([]);
    self.loaded = ko.observable(true);
    var parent = parentViewModel;

    self.buildingBlock = function () {
        return parent.buildingBlock.createChild("path", self.id());
    }
}

var BodyViewModel = function (parentViewModel) {
    var self = this;
    self.isViewModel = ko.observable(true);
    self.id = ko.observable();
    self.description = ko.observable();
    self.example = ko.observable();
    self.loaded = ko.observable(true);
    var parent = parentViewModel;

    self.buildingBlock = function () {
        return parent.buildingBlock.createChild("body", self.id());
    }
}


var VerbViewModel = function (parentViewModel) {
    var self = this;
    self.isViewModel = ko.observable(true);
    self.id = ko.observable();
    self.verb = ko.observable();
    self.parameters = ko.observableArray([]);
    self.statusCodes = ko.observableArray([]);
    self.requestBody = ko.observable();
    self.responseBody = ko.observable(new BodyViewModel(self));
    self.loaded = ko.observable(true);
    var parent = parentViewModel;

    self.buildingBlock = function () {
        return parent.buildingBlock.createChild("verb", self.id());
    }
}


var PageViewModel = function () {
    var self = this;
    self.isViewModel = ko.observable(true);
    self.tableColumnCount = ko.observable(6);
    self.restApis = ko.observableArray();

    self.selectedApi = ko.observable(null);

    self.loading = ko.observable(false);

    self.editMode = ko.observable(false);

    self.saving = ko.observable(false);

    self.refresh = function () {
        var baseUrl = rootBuildingBlock.createChild("restApi").toUrl();
        self.selectedApi(null);
        self.loading(true);
        return $.ajax({
            url: baseUrl,
            dataType: "json",
            accepts: "application/json"
        }).done(function (data) {
            self.restApis.removeAll();
            $.each(data, function (index, item) {
                var viewModel = new RestApiViewModel();
                ko.mapping.fromJS(item, {}, viewModel);
                self.restApis.push(viewModel);
            });
        }).always(function () {
            self.loading(false);
        });
    }


    self.onRowClick = function (apiItem) {
        if (apiItem && (!self.selectedApi() || self.selectedApi() !== apiItem)) {
            apiItem.loadPathGroup();
        } else if (self.selectedApi() && self.selectedApi() === apiItem && !self.editMode()) {
            self.selectedApi(null);
            return;
        }
        self.selectedApi(apiItem);
    }

    self.edit = function (apiItem) {
        self.selectedApi(apiItem);
        self.editMode(true);
    }

    self.cancelEdit = function (apiItem) {
        self.editMode(false);
        self.selectedApi(null);
        if (apiItem)
            apiItem.refresh().done(function () {
                var hasViewModels = true;
                $.each(apiItem.pathGroups(), function (index, item) {
                    if (!item.isViewModel || !item.isViewModel()) {
                        hasViewModels = false;
                    }
                });
                if (!hasViewModels)
                    apiItem.loadPathGroup();
            });
    }

    self.save = function (apiItem) {
        self.saving(true);
        apiItem.save().always(function () {
            self.saving(false);
        });
        self.editMode(false);
    }


    self.refresh();
}