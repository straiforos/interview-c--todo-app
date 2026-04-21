/**
 * Summary DTO for listing tasks.
 */
export interface TaskSummaryDto {
  /** Unique identifier for the task */
  id: number;
  /** Title of the task */
  title: string;
  /** Completion status */
  isCompleted: boolean;
}

/**
 * Detailed DTO for viewing or editing a task.
 */
export interface TaskDto {
  /** Unique identifier for the task */
  id: number;
  /** Title of the task */
  title: string;
  /** Optional detailed description */
  description?: string;
  /** Completion status */
  isCompleted: boolean;
  /** ISO timestamp of when the task was created */
  createdAt: string;
  /** Optional ISO timestamp of the last update */
  updatedAt?: string;
  /** Unique identifier of the user who created the task */
  creatorId: string;
  /** Optional unique identifier of the assigned user */
  assigneeId?: string;
}

/**
 * DTO for creating a new task.
 */
export interface CreateTaskDto {
  /** Title of the task */
  title: string;
  /** Optional detailed description */
  description?: string;
  /** Optional unique identifier of the assigned user */
  assigneeId?: string;
}

/**
 * DTO for updating an existing task.
 */
export interface UpdateTaskDto {
  /** Title of the task */
  title: string;
  /** Optional detailed description */
  description?: string;
  /** Completion status */
  isCompleted: boolean;
  /** Optional unique identifier of the assigned user */
  assigneeId?: string;
}
