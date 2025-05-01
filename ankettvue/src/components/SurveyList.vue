<template>
  <div class="container">
    <h1 class="title">📋 Survey List</h1>

    <!-- Error Alert -->
    <div v-if="rateLimitError" class="error-message">
      ⚠️ Çok fazla istek gönderildi. Lütfen daha sonra tekrar deneyin.
    </div>

    <div v-if="userRole === 'Admin'" class="btn-container">
      <button class="create-btn" @click="$router.push('/create-survey')">➕ Create New Survey</button>
    </div>

    <div class="survey-list">
      <div v-for="survey in surveys" :key="survey.id" class="survey-card">
        <h3>{{ survey.title }}</h3>
        <p class="date">🗓 {{ formatDate(survey.createdAt) }}</p>

        <div class="actions">
          <button v-if="userRole === 'Admin'" class="detail-btn" @click="goToSurveyDetail(survey.id)">
            📊 View Details
          </button>

          <button v-if="userRole === 'Employee' && !completedSurveys.includes(survey.id)"
                  class="solve-btn"
                  @click="takeSurvey(survey.id)">
            ✅ Take Survey
          </button>

          <span v-if="userRole === 'Employee' && completedSurveys.includes(survey.id)" class="completed-badge">
            ✔ Completed
          </span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from "axios";

  export default {
    data() {
      return {
        surveys: [],
        completedSurveys: [],
        userRole: localStorage.getItem("userRole"),
        userId: localStorage.getItem("userId"),
       rateLimitError: false, //api rate limit
      };
    },

    async created() {
      try {
        const surveyResponse = await axios.get("http://localhost:5295/api/survey");
        this.surveys = surveyResponse.data;

        if (this.userRole === "Employee") {
          const completedResponse = await axios.get(`http://localhost:5295/api/survey/completed/${this.userId}`);
          this.completedSurveys = completedResponse.data;
        }
      } catch (error) {//api rate limit
        if (error.response && error.response.status === 429) {
        //  this.rateLimitError = true;
        }
      }
    },

    methods: {
      goToSurveyDetail(surveyId) {
        this.$router.push(`/survey-detail/${surveyId}`);
      },
      takeSurvey(surveyId) {
        this.$router.push(`/take-survey/${surveyId}`);
      },
      formatDate(dateString) {
        return new Date(dateString).toLocaleDateString("en-US");
      },
    },
  };
</script>

<style scoped>
  .container {
    max-width: 800px;
    margin: auto;
    text-align: center;
    padding: 20px;
  }

  .title {
    font-size: 28px;
    font-weight: bold;
    margin-bottom: 20px;
  }

  /* Error message style */
  .error-message {
    background-color: #ffcccc;
    color: #a94442;
    padding: 12px;
    margin-bottom: 20px;
    border-radius: 8px;
    font-weight: bold;
  }

  /* Button for creating a new survey */
  .btn-container {
    text-align: right;
    margin-bottom: 15px;
  }

  .create-btn {
    background-color: #4caf50;
    color: white;
    padding: 10px 15px;
    border: none;
    border-radius: 8px;
    font-size: 16px;
    cursor: pointer;
    transition: 0.3s;
  }

    .create-btn:hover {
      background-color: #45a049;
    }

  /* Survey Cards */
  .survey-list {
    display: flex;
    flex-direction: column;
    gap: 15px;
  }

  .survey-card {
    background: #ffffff;
    padding: 15px;
    border-radius: 10px;
    box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.1);
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    transition: 0.3s;
  }

    .survey-card:hover {
      transform: scale(1.02);
    }

  .date {
    font-size: 14px;
    color: #777;
    margin-bottom: 10px;
  }

  .actions {
    display: flex;
    gap: 10px;
  }

  .detail-btn,
  .solve-btn {
    padding: 8px 12px;
    border-radius: 6px;
    font-size: 14px;
    cursor: pointer;
    transition: 0.3s;
    border: none;
  }

  .detail-btn {
    background-color: #3498db;
    color: white;
  }

    .detail-btn:hover {
      background-color: #2980b9;
    }

  .solve-btn {
    background-color: #f39c12;
    color: white;
  }

    .solve-btn:hover {
      background-color: #e67e22;
    }

  .completed-badge {
    background-color: #2ecc71;
    color: white;
    padding: 6px 10px;
    border-radius: 6px;
    font-size: 12px;
    font-weight: bold;
  }
</style>
