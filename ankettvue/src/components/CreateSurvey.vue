<template>
  <div class="create-survey-container">
    <h2>Create New Survey</h2>

    <!-- Survey Title -->
    <div class="form-group">
      <label for="title">Survey Title</label>
      <input v-model="title" id="title" placeholder="Survey Title" class="input-field" />
      <div v-if="errors.title" class="error-message">{{ errors.title }}</div>
    </div>

    <!-- Questions and Options -->
    <div v-for="(question, index) in questions" :key="index" class="question-group">
      <div class="form-group">
        <label :for="'question' + index">Question {{ index + 1 }}</label>
        <input v-model="question.text" :id="'question' + index" placeholder="Question text" class="input-field" />
        <button @click="removeQuestion(index)" class="remove-btn">Remove</button>
        <div v-if="errors[`question${index}`]" class="error-message">{{ errors[`question${index}`] }}</div>
      </div>

      <div v-for="(option, optIndex) in question.options" :key="optIndex" class="option-group">
        <div class="form-group">
          <label :for="'option' + optIndex">Option {{ optIndex + 1 }}</label>
          <input v-model="option.text" :id="'option' + optIndex" placeholder="Option text" class="input-field" />
          <button @click="removeOption(index, optIndex)" class="remove-btn">Remove</button>
          <div v-if="errors[`option${index}-${optIndex}`]" class="error-message">{{ errors[`option${index}-${optIndex}`] }}</div>
        </div>
      </div>

      <button @click="addOption(index)" class="add-option-btn">+ Add Option</button>
    </div>

    <button @click="addQuestion" class="add-question-btn">+ Add Question</button>
    <button @click="createSurvey" class="submit-btn">Save Survey</button>
  </div>
</template>

<script>
  import axios from "axios";

  export default {
    data() {
      return {
        title: "",
        questions: [],
        errors: {}, // To store error messages
      };
    },
    methods: {
      addQuestion() {
        this.questions.push({ text: "", options: [{ text: "" }] });
        this.scrollToBottom();
      },
      removeQuestion(index) {
        this.questions.splice(index, 1);
      },
      addOption(questionIndex) {
        this.questions[questionIndex].options.push({ text: "" });
        this.scrollToBottom();
      },
      removeOption(questionIndex, optionIndex) {
        this.questions[questionIndex].options.splice(optionIndex, 1);
      },
      validateSurvey() {
        let isValid = true;
        this.errors = {}; // Reset errors

        if (!this.title.trim()) {
          this.errors.title = "Survey title is required.";
          isValid = false;
        }

        this.questions.forEach((question, index) => {
          if (!question.text.trim()) {
            this.errors[`question${index}`] = `Question ${index + 1} is required.`;
            isValid = false;
          }
          question.options.forEach((option, optIndex) => {
            if (!option.text.trim()) {
              this.errors[`option${index}-${optIndex}`] = `Option ${optIndex + 1} for Question ${index + 1} is required.`;
              isValid = false;
            }
          });
        });

        return isValid;
      },
      async createSurvey() {
        if (!this.validateSurvey()) {
          return;
        }

        const requestBody = {
          title: this.title,
          questions: this.questions.map(question => ({
            text: question.text,
            options: question.options.map(option => ({
              text: option.text,
            })),
          })),
        };

        console.log('Sent data:', JSON.stringify(requestBody));

        try {
          const response = await axios.post("http://localhost:5295/api/survey/create", requestBody);
          this.$message.success('The survey was created successfully.');
          this.$router.push("/survey-list");
        } catch (error) {
          if (error.response && error.response.data && error.response.data.Message) {
            this.$message.error(error.response.data.Message);
          } else {
            this.$message.error('Could not create survey. Please try again.');
          }
        }
      },
      scrollToBottom() {
        this.$nextTick(() => {
          const container = document.querySelector('.create-survey-container');
          container.scrollTop = container.scrollHeight;
        });
      },
    },
  };
</script>

<style scoped>
  .create-survey-container {
    max-width: 800px;
    margin: 0 auto;
    padding: 30px;
    background-color: rgba(255, 255, 255, 0.9);
    border-radius: 10px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    font-family: 'Arial', sans-serif;
    color: #333;
    overflow-y: auto;
    height: 80vh;
  }

  h2 {
    text-align: center;
    color: #6a11cb;
    font-size: 2em;
    margin-bottom: 20px;
  }

  .form-group {
    margin-bottom: 20px;
  }

  label {
    font-size: 1.1em;
    margin-bottom: 8px;
    color: #555;
  }

  .input-field {
    width: 100%;
    padding: 10px;
    border-radius: 5px;
    border: 1px solid #ddd;
    margin-bottom: 10px;
    font-size: 1em;
  }

    .input-field:focus {
      border-color: #6a11cb;
      outline: none;
    }

  button {
    padding: 10px 20px;
    background-color: #2575fc;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 1em;
    margin-top: 10px;
  }

    button:hover {
      background-color: #6a11cb;
    }

  .add-question-btn,
  .add-option-btn {
    background-color: #28a745;
  }

  .remove-btn {
    background-color: #dc3545;
  }

  button:focus {
    outline: none;
  }

  .error-message {
    color: red;
    font-size: 0.9em;
    margin-top: 5px;
  }

  /* Responsive Design */
  @media (max-width: 768px) {
    .create-survey-container {
      padding: 20px;
    }

    h2 {
      font-size: 1.5em;
    }
  }
</style>
