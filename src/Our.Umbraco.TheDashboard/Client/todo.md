
# TODO
* [x] Use extension points for widgets 
* [ ] Widget-thumbnails? For when adding new widgets.
* [ ] Interaction to add new widget to the dashboard
* [ ] Storing the configuration (maybe each user can have multiple configs?)
* [ ] Could we make a component update it's own height (expandable?)
* [ ] Support for "instances"? E.g for HTML Widget, might want to add existing one.
* [ ] Rendering widgets notification

WidgetPlacement
* Actual height/ width.
* Reference to the widget

Widget
* Overrides for default settings
* Permissions

WidgetType
* Default settings for height, width etc
* Default permissons: admins vs everyone

Notifications
* When loading dashboard
* When loading "add new" view

## Ideas
* Store/configure a default layout for new users. Also defaults based on User Groups?
  * Maybe use a key/value store for stuff? `config_1` = user 1s config, `default_22` default for yada, `html` html for the "html widget". 
* Widget with HTML to save? (Like a edit button, paste HTML and save), global or personal 
* Widget showing log-entries (what about permissions?)
* User-level TODO-widget?


Icons
https://thenounproject.com/browse/icons/term/three-dots/
