import { createApp } from 'vue';
import App from './App.vue';
import { createRouter, createWebHistory } from 'vue-router';
import SurveyList from './components/SurveyList.vue';
import Login from './components/Login.vue';
import SurveyDetail from './components/SurveyDetail.vue';

// Vue Router'ı kurma
const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/login' },
    { path: '/login', component: Login },  // Login sayfası rotası
    { path: '/survey-list', component: SurveyList },
    { path: '/survey/:surveyId', component: SurveyDetail },
  ],
});

const app = createApp(App);
app.use(router);
app.mount('#app');
