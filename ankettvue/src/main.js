import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import Vue from 'vue';
import Router from 'vue-router';
import axios from 'axios';
import Login from './components/Login.vue';
import SurveyList from './components/SurveyList.vue';
import SurveyDetail from './components/SurveyDetail.vue';

axios.defaults.baseURL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5295';
Vue.use(Router);
const app = createApp(App);
app.use(router);
app.mount('#app');
export default new Router({
  routes: [
    { path: '/', component: SurveyList },
    { path: '/login', component: Login },
    { path: '/survey-list', component: SurveyList },
    { path: '/survey/:surveyId', component: SurveyDetail },
  ],
});
createApp(App).mount('#app')
