import {manifests as counterManifests} from './counters/manifests';
import {manifests as myRecentActivitiesManifest} from './my-activities/manifests';
import {manifests as recentActivitiesManifests} from './recent-activities/manifests';
import {manifests as pendingChangesManifests} from './pending-changes/manifests';

export const manifests = [
  ...counterManifests,
  ...myRecentActivitiesManifest,
  ...recentActivitiesManifests,
  ...pendingChangesManifests
]
