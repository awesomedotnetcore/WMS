<template>
  <div class="editable-cell">
    <div v-if="editable" class="editable-cell-input-wrapper">
      <a-input v-if="type != 'number'" :type="type" style="width: 70%" size="small" :value="value" @change="handleChange" @pressEnter="check" v-bind="$attrs" />
      <a-input-number v-else style="width: 70%" size="small" :value="value" @change="handleChange" @pressEnter="check" v-bind="$attrs" />
      <a-icon type="check" class="editable-cell-icon-check" @click="check" />
    </div>
    <div v-else class="editable-cell-text-wrapper">
      {{ value || ' ' }}
      <a-icon type="edit" v-if="!disabled" class="editable-cell-icon" @click="edit" />
    </div>
  </div>
</template>
<script>
export default {
  props: {
    text: { type: [Number, String], default: null, required: true },
    type: { type: String, default: 'text', required: false },
    disabled: { type: Boolean, default: false, required: false }
  },
  data() {
    return {
      value: this.text,
      editable: false
    }
  },
  methods: {
    handleChange(e) {
      if (this.type === 'number') {
        this.value = e
      } else {
        const value = e.target.value
        this.value = value
      }
    },
    check() {
      this.editable = false
      this.$emit('change', this.value)
    },
    edit() {
      this.editable = true
    }
  }
}
</script>