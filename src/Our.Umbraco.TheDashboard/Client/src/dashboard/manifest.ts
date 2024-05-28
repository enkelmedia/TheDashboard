
const dashboard = 	{
    type: 'dashboard',
    alias: 'TheDashboard.Dashboard',
    name: 'The Dashboard Dashboard',
    weight: 10000,
    element : ()=> import('./the-dashboard.element.js'),
    meta: {
        label: '#theDashboard_tabLabel',
        pathname: 'dashboard'
    },
    conditions : [
        {
            alias : "Umb.Condition.SectionAlias",
            match : "Umb.Section.Content"
        }
    ]
};

export const manifests = [
    dashboard
]
