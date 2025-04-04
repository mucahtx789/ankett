import { createApp } from 'vue';
import App from './App.vue';
import { createRouter, createWebHistory } from 'vue-router';
import SurveyList from './components/SurveyList.vue';
import Login from './components/Login.vue';
import Register from './components/Register.vue';
import SurveyDetail from './components/SurveyDetail.vue';
import CreateSurvey from './components/CreateSurvey.vue';
import TakeSurvey from './components/TakeSurvey.vue';


// Vue Router'ı kurma
const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/login' },
    { path: '/login', component: Login },  // Login sayfası rotası
    { path: '/register', component: Register },
    { path: '/survey-list', component: SurveyList },
    { path: '/survey-detail/:surveyId', component: SurveyDetail },
    { path: '/create-survey', component: CreateSurvey },
    { path: '/take-survey/:id', component: TakeSurvey },
  ],
});

const app = createApp(App);
app.use(router);
app.mount('#app');
