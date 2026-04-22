import { createBrowserRouter, RouterProvider, Navigate } from 'react-router';
import { tasksLoader, taskDetailLoader, taskCreateLoader, taskEditLoader } from './loaders';
import { Layout } from './components/Layout';
import { LoginPage } from './pages/LoginPage';
import { RegisterPage } from './pages/RegisterPage';
import { TaskListPage } from './pages/TaskListPage';
import { TaskDetailPage } from './pages/TaskDetailPage';
import { TaskFormPage } from './pages/TaskFormPage';
import { ProfilePage } from './pages/ProfilePage';
import { ErrorBoundary } from './components/ErrorBoundary';

let router: ReturnType<typeof createBrowserRouter>;

export default function App() {
  if (!router) {
    router = createBrowserRouter([
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
                loader: taskCreateLoader,
                element: <TaskFormPage />,
              },
              {
                path: 'tasks/:id',
                loader: taskDetailLoader,
                element: <TaskDetailPage />,
              },
              {
                path: 'tasks/:id/edit',
                loader: taskEditLoader,
                element: <TaskFormPage />,
              },
              {
                path: 'profile',
                element: <ProfilePage />,
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
  }

  return <RouterProvider router={router} />;
}
