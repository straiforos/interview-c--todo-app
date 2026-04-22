import { createBrowserRouter, RouterProvider, Navigate } from 'react-router';
import { tasksLoader, taskDetailLoader } from './loaders';
import { Layout } from './components/Layout';
import { LoginPage } from './pages/LoginPage';
import { RegisterPage } from './pages/RegisterPage';
import { TaskListPage } from './pages/TaskListPage';
import { TaskDetailPage } from './pages/TaskDetailPage';
import { TaskFormPage } from './pages/TaskFormPage';
import { ErrorBoundary } from './components/ErrorBoundary';

const router = createBrowserRouter([
  {
    path: '/login',
    element: <LoginPage />,
  },
  {
    path: '/register',
    element: <RegisterPage />,
  },
  {
    path: '/',
    element: <Layout />,
    children: [
      {
        errorElement: <ErrorBoundary />,
        children: [
          {
            index: true,
            element: <Navigate to="/tasks" replace />,
          },
          {
            path: 'tasks',
            loader: tasksLoader,
            element: <TaskListPage />,
          },
          {
            path: 'tasks/new',
            element: <TaskFormPage />,
          },
          {
            path: 'tasks/:id',
            loader: taskDetailLoader,
            element: <TaskDetailPage />,
          },
          {
            path: 'tasks/:id/edit',
            loader: taskDetailLoader,
            element: <TaskFormPage />,
          },
          {
            path: '*',
            element: <ErrorBoundary />,
            loader: () => {
              throw new Response('Not Found', { status: 404 });
            }
          },
        ],
      },
    ],
  },
]);

export default function App() {
  return <RouterProvider router={router} />;
}
